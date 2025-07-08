using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Provides methods for displaying paginated lists in the console application.
/// </summary>
internal static class PaginatedLists
{
    private static readonly string _consolePrefix = "PaginatedList> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    public static readonly int PageSize = 10; // Number of items per page

    /// <summary>
    /// Displays a paginated list of items and allows the user to select one item by number.
    /// </summary>
    /// <param name="items">List of items to display.</param>
    /// <param name="title">Title of the list for display purposes.</param>
    /// <returns>Number of the selected item, or null if cancelled.</returns>
    public static int? PaginatedListWithSelection(List<string> items, string title)
    {
        if (items.Count == 0)
        {
            _console.WriteLine($"No {title} found.");
            return null;
        }

        int page = 0;
        int totalPages = (int)Math.Ceiling((double)items.Count / PageSize);
        while (true)
        {
            Console.WriteLine();
            _console.WriteLine($"--- {title} (Page {page + 1}/{totalPages}) ---");

            var pageItems = items.Skip(page * PageSize).Take(PageSize).ToList();
            for (int i = 0; i < pageItems.Count; i++)
            {
                _console.WriteLine($"{i + 1}. {pageItems[i]}");
            }

            Console.WriteLine();
            _console.WriteLine("Type number to select or 'n' for next page, 'p' for previous, 'q' to cancel:");
            string? input = _console.ReadLine()?.Trim().ToLower();

            if (input == "q")
                return null;
            else if (input == "n" && (page + 1) * PageSize < items.Count)
                page++;
            else if (input == "p" && page > 0)
                page--;
            else if (int.TryParse(input, out int choice))
            {
                int absoluteIndex = page * PageSize + (choice - 1);
                if (choice >= 1 && choice <= pageItems.Count && absoluteIndex < items.Count)
                    return absoluteIndex;
                else
                    _console.WriteLine("Invalid selection.");
            }
            else
            {
                _console.WriteLine("Invalid input or page.");
            }
        }
    }

    /// <summary>
    /// Displays a paginated list of items with an option to delete items by number.
    /// </summary>
    /// <param name="items">List of items to display.</param>
    /// <param name="title">Title of the list for display purposes.</param>
    /// <param name="onDelete">Action to call when an item is deleted, receiving the index of the item in the original list.</param>
    public static void PaginatedListWithDeletion(List<string> items, string title, Action<int>? onDelete = null)
    {
        if (items.Count == 0)
        {
            _console.WriteLine($"No {title} found.");
            return;
        }

        int page = 0;
        int totalPages = (int)Math.Ceiling((double)items.Count / PageSize);
        while (true)
        {
            Console.WriteLine();
            _console.WriteLine($"--- {title} (Page {page + 1}/{totalPages}) ---");

            var pageItems = items.Skip(page * PageSize).Take(PageSize).ToList();
            for (int i = 0; i < pageItems.Count; i++)
            {
                _console.WriteLine($"{i + 1}. {pageItems[i]}");
            }

            Console.WriteLine();
            _console.WriteLine("Type 'n' for next page, 'p' for previous, 'q' to quit.");
            if (onDelete != null)
                _console.WriteLine("Type 'delete (number)' to remove an item.");
            string? input = _console.ReadLine()?.Trim().ToLower();
            if (input == "q") break;
            else if (input == "n" && (page + 1) * PageSize < items.Count)
                page++;
            else if (input == "p" && page > 0)
                page--;
            // Deletion command
            else if (input != null && input.StartsWith("delete") && onDelete != null)
            {
                // Get index to delete
                var parts = input.Split(' ');
                if (parts.Length == 2 && int.TryParse(parts[1], out int index))
                {
                    int absoluteIndex = page * PageSize + index - 1;
                    if (absoluteIndex >= 0 && absoluteIndex < items.Count)
                    {
                        // Call deletion action
                        onDelete(absoluteIndex);
                        items.RemoveAt(absoluteIndex);
                        totalPages = (int)Math.Ceiling((double)items.Count / PageSize);
                        if (page >= totalPages) page = Math.Max(0, totalPages - 1);
                    }
                    else
                    {
                        _console.WriteLine("Invalid index.");
                    }
                }
                else
                {
                    _console.WriteLine("Invalid delete command. Use: delete {number}");
                }
            }
        }
    }
}
