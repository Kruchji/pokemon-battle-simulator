using System;

namespace PokemonBattleSimulator;

internal static class Prompts
{
    private static readonly string _consolePrefix = "Prompt> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    public static readonly string AbortCommand = "abort";

    // For general validation
    public static string PromptUntilValid(string prompt, Func<string, bool> validator, string errorMessage)
    {
        while (true)
        {
            _console.WriteLine(prompt);
            string? input = _console.ReadLine();
            if (string.Equals(input, AbortCommand, StringComparison.OrdinalIgnoreCase)) return null!;
            if (input != null && validator(input)) return input;
            _console.WriteLine(errorMessage);
        }
    }

    // For int validation and parsing
    public static int? PromptUntilValidInt(string prompt, Func<string, (bool success, int value)> parser, string errorMessage)
    {
        while (true)
        {
            _console.WriteLine(prompt);
            string? input = _console.ReadLine();
            if (string.Equals(input, AbortCommand, StringComparison.OrdinalIgnoreCase)) return null;
            var (success, value) = parser(input ?? "");
            if (success) return value;
            _console.WriteLine(errorMessage);
        }
    }

    // For enum validation and parsing
    public static T? PromptEnum<T>(string prompt) where T : struct, Enum
    {
        while (true)
        {
            _console.WriteLine(prompt);
            _console.WriteLine($"Available: {string.Join(", ", Enum.GetNames(typeof(T)))}");
            string? input = _console.ReadLine();
            if (string.Equals(input, AbortCommand, StringComparison.OrdinalIgnoreCase)) return null;
            if (!int.TryParse(input, out _) && Enum.TryParse<T>(input, true, out var result)) return result;
            _console.WriteLine("Invalid choice. Try again.");
        }
    }

    // Prompt for enum with optional skip
    public static (T?, bool empty) PromptEnumOptional<T>(string prompt) where T : struct, Enum
    {
        while (true)
        {
            _console.WriteLine(prompt);
            _console.WriteLine($"Available: {string.Join(", ", Enum.GetNames(typeof(T)))}");
            string? input = _console.ReadLine();
            if (string.Equals(input, AbortCommand, StringComparison.OrdinalIgnoreCase)) return (null, false);
            if (string.IsNullOrWhiteSpace(input)) return (null, true); // User pressed Enter without input
            if (!int.TryParse(input, out _) && Enum.TryParse<T>(input, true, out var result)) return (result, false);
            _console.WriteLine("Invalid choice. Try again.");
        }
    }

    public static List<Move>? PromptMoveSelection(User user)
    {
        var moves = user.Moves;
        int page = 0;
        int totalPages = (moves.Count + PaginatedLists.PageSize - 1) / PaginatedLists.PageSize;
        var selected = new List<Move>();

        while (selected.Count < Pokemon.NumberOfMoves)
        {
            Console.WriteLine();
            _console.WriteLine($"--- Moves (Page {page + 1}/{totalPages}) ---");
            var pageMoves = moves.Skip(page * PaginatedLists.PageSize).Take(PaginatedLists.PageSize).ToList();

            // List all moves
            for (int i = 0; i < pageMoves.Count; i++)
            {
                _console.WriteLine($"{i + 1}. {pageMoves[i].Name} ({pageMoves[i].MoveType}, {pageMoves[i].Category}, ATK: {pageMoves[i].Power}, ACC: {pageMoves[i].Accuracy}, PP: {pageMoves[i].PP})");
            }

            // Next/prev page message
            _console.WriteLine("Type 'n' for next page, 'p' for previous page.");
            _console.WriteLine("Type 'done' when you're finished selecting moves (at least one required).");
            _console.WriteLine($"Currently selected moves: {string.Join(", ", selected.Select(m => m.Name))}");

            _console.WriteLine("Select move by number:");
            string? input = _console.ReadLine();

            if (string.Equals(input, AbortCommand, StringComparison.OrdinalIgnoreCase)) return null;

            // End selection
            if (string.Equals(input, "done", StringComparison.OrdinalIgnoreCase))
            {
                if (selected.Count >= 1) break;
                _console.WriteLine("You must select at least one move.");
                continue;
            }

            // Move to next page
            if (string.Equals(input, "n", StringComparison.OrdinalIgnoreCase))
            {
                if (page < totalPages - 1)
                {
                    page++;
                }
                else
                {
                    _console.WriteLine("You are already on the last page.");
                }
                continue;
            }

            // Move to previous page
            if (string.Equals(input, "p", StringComparison.OrdinalIgnoreCase))
            {
                if (page > 0)
                {
                    page--;
                }
                else
                {
                    _console.WriteLine("You are already on the first page.");
                }
                continue;
            }

            if (int.TryParse(input, out int index) && index >= 1 && index <= pageMoves.Count)
            {
                var move = pageMoves[index - 1];

                selected.Add(move);
                _console.WriteLine($"Added: {move.Name}");
            }
            else
            {
                _console.WriteLine("Invalid move index. Please try again.");
            }
        }

        return selected;
    }


    public static List<Pokemon>? PromptPokemonSelection(User user)
    {
        var pokemons = user.PokemonList;
        int page = 0;
        int totalPages = (pokemons.Count + PaginatedLists.PageSize - 1) / PaginatedLists.PageSize;
        var selected = new List<Pokemon>();

        while (selected.Count < PokemonTeam.MaxTeamSize)
        {
            Console.WriteLine();
            _console.WriteLine($"--- Pokemon (Page {page + 1}/{totalPages}) ---");

            var pagePokemon = pokemons.Skip(page * PaginatedLists.PageSize).Take(PaginatedLists.PageSize).ToList();

            for (int i = 0; i < pagePokemon.Count; i++)
            {
                var pkmn = pagePokemon[i];
                _console.WriteLine($"{i + 1}. {pkmn.Name} ({pkmn.FirstType}" + (pkmn.SecondType.HasValue ? $"/{pkmn.SecondType}" : "") + $", LV: {pkmn.Level})");
            }

            _console.WriteLine("Type 'n' for next page, 'p' for previous page.\n");
            _console.WriteLine($"Type 'done' when you're finished selecting Pokémon (1 to {PokemonTeam.MaxTeamSize}).");
            _console.WriteLine($"Current team: {string.Join(", ", selected.Select(p => p.Name))}\n");

            _console.WriteLine("Select Pokémon by number:");
            string? input = _console.ReadLine();

            if (string.Equals(input, AbortCommand, StringComparison.OrdinalIgnoreCase)) return null;

            if (string.Equals(input, "done", StringComparison.OrdinalIgnoreCase))
            {
                if (selected.Count >= 1) break;
                _console.WriteLine("You must select at least one Pokémon.");
                continue;
            }

            if (string.Equals(input, "n", StringComparison.OrdinalIgnoreCase))
            {
                if (page < totalPages - 1)
                {
                    page++;
                }
                else
                {
                    _console.WriteLine("You are already on the last page.");
                }
                continue;
            }

            if (string.Equals(input, "p", StringComparison.OrdinalIgnoreCase))
            {
                if (page > 0)
                {
                    page--;
                }
                else
                {
                    _console.WriteLine("You are already on the first page.");
                }
                continue;
            }

            if (int.TryParse(input, out int index) && index >= 1 && index <= pagePokemon.Count)
            {
                var selectedPokemon = pagePokemon[index - 1];

                selected.Add(selectedPokemon);
                _console.WriteLine($"Added: {selectedPokemon.Name}");
            }
            else
            {
                _console.WriteLine("Invalid input. Please enter a valid number or command.");
            }
        }

        return selected;
    }

}
