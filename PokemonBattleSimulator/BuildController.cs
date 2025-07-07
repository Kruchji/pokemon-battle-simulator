using System;

namespace PokemonBattleSimulator;

internal class BuildController : IController
{
    private static readonly string _consolePrefix = "BuildMenu> ";
    private readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    public void Run(User user)
    {
        Console.WriteLine();
        _console.WriteLine("Welcome to the Build Menu!");
        _console.WriteLine("Here you can create and manage your Pokemon and Pokemon teams.\n");

        while (true)
        {
            _console.WriteLine("Type 'back' to return to the main menu.");
            _console.WriteLine("Type 'newM', 'newP' to create a new Move or Pokemon respectively.");
            _console.WriteLine("Type 'listM', 'listP', 'listT' to view paginated list of your Moves, Pokemon or Teams respectively.");
            _console.WriteLine("Type 'defaults' to load default Pokemon and Moves.\n");

            var userInput = _console.ReadLine()?.Trim().ToLower();

            switch (userInput)
            {
                case "back":
                    return;
                case "newm":
                    BuildManager.CreateMove(user);
                    break;
                case "newp":
                    BuildManager.CreatePokemon(user);
                    break;
                case "listm":
                    PaginateList(user.Moves.Select(m => $"{m.Name} ({m.MoveType}, {m.Category}, ATK: {m.Power}, ACC: {m.Accuracy}, PP: {m.PP})").ToList(), "Moves");
                    break;
                case "listp":
                    PaginateList(user.PokemonList.Select(p => $"{p.Name} ({p.FirstType}" + (p.SecondType.HasValue ? $"/{p.SecondType}" : "") + $", LV: {p.Level})").ToList(), "Pokemon");
                    break;
                case "listt":
                    PaginateList(user.PokemonTeams.Select(t => $"{t.Name} (Contains: " + string.Join(", ", t.PokemonList.Select(p => p?.Name ?? "empty")) + ")").ToList(), "Pokemon Teams");
                    break;
                case "defaults":
                    _console.WriteLine("Loading default Pokemon and Moves...");
                    BuildManager.LoadDefaults(user);
                    _console.WriteLine("Default Pokemon and Moves loaded successfully.");
                    break;
                default:
                    _console.WriteLine("Invalid command. Please type 'back' to return to the main menu.\n");
                    break;
            }
        }
    }

    private void PaginateList(List<string> items, string title)
    {
        const int pageSize = 10;
        if (items.Count == 0)
        {
            _console.WriteLine($"No {title} found.");
            return;
        }

        int page = 0;
        int totalPages = (int)Math.Ceiling((double)items.Count / pageSize);
        while (true)
        {
            Console.WriteLine();
            _console.WriteLine($"--- {title} (Page {page + 1}/{totalPages}) ---");

            var pageItems = items.Skip(page * pageSize).Take(pageSize).ToList();
            for (int i = 0; i < pageItems.Count; i++)
            {
                _console.WriteLine($"{i + 1}. {pageItems[i]}");
            }

            Console.WriteLine();
            _console.WriteLine("Type 'n' for next page, 'p' for previous, 'q' to quit.");
            string? input = _console.ReadLine()?.Trim().ToLower();
            if (input == "q") break;
            else if (input == "n" && (page + 1) * pageSize < items.Count)
                page++;
            else if (input == "p" && page > 0)
                page--;
        }
    }
}
