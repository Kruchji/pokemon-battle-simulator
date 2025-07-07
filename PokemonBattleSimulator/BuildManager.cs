using System;

namespace PokemonBattleSimulator;

internal static class BuildManager
{
    private static readonly string _consolePrefix = "BuildManager> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);

    private static readonly string _abortCommand = "abort";

    public static void CreateMove(User user)
    {
        _console.WriteLine("Let's create a new Pokemon move.");
        _console.WriteLine($"You can type '{_abortCommand}' at any time to cancel the process.\n");

        string name = PromptUntilValid("Enter move name:", s => !string.IsNullOrWhiteSpace(s), "Name cannot be empty.");
        if (name == null) return;

        int? power = PromptUntilValid("Enter move power (0-250):", TryParseIntInRange(0, 250), "Power must be a number between 0 and 250.");
        if (power == null) return;

        int? accuracy = PromptUntilValid("Enter move accuracy (1-100):", TryParseIntInRange(1, 100), "Accuracy must be between 1 and 100.");
        if (accuracy == null) return;

        int? pp = PromptUntilValid("Enter move PP (1-40):", TryParseIntInRange(1, 40), "PP must be between 1 and 40.");
        if (pp == null) return;

        PokemonType? moveType = PromptEnum<PokemonType>("Enter move type (e.g., Fire, Water):");
        if (moveType == null) return;

        MoveCategory? category = PromptEnum<MoveCategory>("Enter move category (e.g., Physical, Special, Status):");
        if (category == null) return;

        var move = new Move(name, (int)power, (int)accuracy, (int)pp, (PokemonType)moveType, (MoveCategory)category);
        user.AddMove(move);

        Console.WriteLine();
        _console.WriteLine($"Move '{name}' created successfully!");
    }


    public static void CreatePokemon(User user)
    {
        // Guides user to enter all details and constructs a new Pokemon
        if (user.Moves.Count == 0)
        {
            _console.WriteLine("You must create at least one move before creating a Pokémon.");
            return;
        }

        _console.WriteLine("Let's create a new Pokémon.");
        _console.WriteLine($"You can type '{_abortCommand}' at any time to cancel the process.\n");

        string name = PromptUntilValid("Enter Pokémon name:", s => !string.IsNullOrWhiteSpace(s), "Name cannot be empty.");
        if (name == null) return;

        // Define stats

        int? level = PromptUntilValid("Enter level (1-100):", TryParseIntInRange(1, 100), "Level must be between 1 and 100.");
        if (level == null) return;

        int? health = PromptUntilValid("Enter health (1-999):", TryParseIntInRange(1, 999), "Health must be between 1 and 999.");
        if (health == null) return;

        int? attack = PromptUntilValid("Enter attack (1-999):", TryParseIntInRange(1, 999), "Attack must be between 1 and 999.");
        if (attack == null) return;

        int? defense = PromptUntilValid("Enter defense (1-999):", TryParseIntInRange(1, 999), "Defense must be between 1 and 999.");
        if (defense == null) return;

        int? speed = PromptUntilValid("Enter speed (1-999):", TryParseIntInRange(1, 999), "Speed must be between 1 and 999.");
        if (speed == null) return;

        int? spAttack = PromptUntilValid("Enter special attack (1-999):", TryParseIntInRange(1, 999), "Special Attack must be between 1 and 999.");
        if (spAttack == null) return;

        int? spDefense = PromptUntilValid("Enter special defense (1-999):", TryParseIntInRange(1, 999), "Special Defense must be between 1 and 999.");
        if (spDefense == null) return;

        // Define types

        PokemonType? firstType = PromptEnum<PokemonType>("Enter first type:");
        if (firstType == null) return;

        (PokemonType? secondType, bool emptySecondType)  = PromptEnumOptional<PokemonType>("Enter second type (or press Enter to skip):");
        if (secondType == null && !emptySecondType) return;

        // Move selection

        Console.WriteLine();
        _console.WriteLine("Select at least one move (up to 4):");
        var selectedMoves = PromptMoveSelection(user);
        if (selectedMoves == null || selectedMoves.Count == 0) return;

        // Create Pokémon

        var pokemon = new Pokemon(
            name, (int)level, (int)health, (int)attack, (int)defense, (int)speed, (int)spAttack, (int)spDefense,
            selectedMoves[0], (PokemonType)firstType, secondType
        );

        for (int i = 1; i < selectedMoves.Count; i++)
        {
            pokemon.SetMove(i, selectedMoves[i]);
        }

        user.PokemonList.Add(pokemon);

        Console.WriteLine();
        _console.WriteLine($"Pokémon '{name}' created successfully!");
    }

    // TODO: Move these to separate class?

    // For general validation
    private static string PromptUntilValid(string prompt, Func<string, bool> validator, string errorMessage)
    {
        while (true)
        {
            _console.WriteLine(prompt);
            string? input = _console.ReadLine();
            if (string.Equals(input, _abortCommand, StringComparison.OrdinalIgnoreCase)) return null!;
            if (input != null && validator(input)) return input;
            _console.WriteLine(errorMessage);
        }
    }

    // For int validation
    private static int? PromptUntilValid(string prompt, Func<string, (bool success, int value)> parser, string errorMessage)
    {
        while (true)
        {
            _console.WriteLine(prompt);
            string? input = _console.ReadLine();
            if (string.Equals(input, _abortCommand, StringComparison.OrdinalIgnoreCase)) return null;
            var (success, value) = parser(input ?? "");
            if (success) return value;
            _console.WriteLine(errorMessage);
        }
    }

    private static T? PromptEnum<T>(string prompt) where T : struct, Enum
    {
        while (true)
        {
            _console.WriteLine(prompt);
            _console.WriteLine($"Available: {string.Join(", ", Enum.GetNames(typeof(T)))}");
            string? input = _console.ReadLine();
            if (string.Equals(input, _abortCommand, StringComparison.OrdinalIgnoreCase)) return null;
            if (!int.TryParse(input, out _) && Enum.TryParse<T>(input, true, out var result)) return result;
            _console.WriteLine("Invalid choice. Try again.");
        }
    }

    private static (T?, bool empty) PromptEnumOptional<T>(string prompt) where T : struct, Enum
    {
        while (true)
        {
            _console.WriteLine(prompt);
            _console.WriteLine($"Available: {string.Join(", ", Enum.GetNames(typeof(T)))}");
            string? input = _console.ReadLine();
            if (string.Equals(input, _abortCommand, StringComparison.OrdinalIgnoreCase)) return (null, false);
            if (string.IsNullOrWhiteSpace(input)) return (null, true); // User pressed Enter without input
            if (!int.TryParse(input, out _) && Enum.TryParse<T>(input, true, out var result)) return (result, false);
            _console.WriteLine("Invalid choice. Try again.");
        }
    }

    private static Func<string, (bool, int)> TryParseIntInRange(int min, int max)
    {
        return input =>
        {
            bool parsed = int.TryParse(input, out int value);
            return (parsed && value >= min && value <= max, value);
        };
    }

    private static List<Move>? PromptMoveSelection(User user)
    {
        const int pageSize = 10;
        var moves = user.Moves;
        int page = 0;
        int totalPages = (moves.Count + pageSize - 1) / pageSize;
        var selected = new List<Move>();

        while (selected.Count < Pokemon.NumberOfMoves)
        {
            Console.WriteLine();
            _console.WriteLine($"Page {page + 1}/{totalPages}:");
            var pageMoves = moves.Skip(page * pageSize).Take(pageSize).ToList();

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
            if (string.Equals(input, _abortCommand, StringComparison.OrdinalIgnoreCase)) return null;

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


    public static void LoadDefaults(User user)
    {
        user.ClearAllData();

        // TODO: Create some default Pokemon (with real stats) and Moves (and add to Pokemon) and add them to the user
        var thunderbolt = new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special);
        var flamethrower = new Move("Flamethrower", 90, 100, 15, PokemonType.Fire, MoveCategory.Special);
        var vineWhip = new Move("Vine Whip", 45, 100, 25, PokemonType.Grass, MoveCategory.Physical);
        var scratch = new Move("Scratch", 40, 100, 35, PokemonType.Normal, MoveCategory.Physical);
        var tackle = new Move("Tackle", 40, 50, 35, PokemonType.Normal, MoveCategory.Physical);

        var pikachu = new Pokemon("Pikachu", 35, 55, 40, 50, 50, 90, 10, tackle, PokemonType.Electric);
        var charmander = new Pokemon("Charmander", 39, 52, 43, 60, 50, 65, 10, tackle, PokemonType.Fire);
        var bulbasaur = new Pokemon("Bulbasaur", 45, 49, 49, 65, 65, 45, 10, tackle, PokemonType.Grass);

        pikachu.SetMove(1, thunderbolt);
        pikachu.SetMove(2, scratch);

        charmander.SetMove(1, flamethrower);
        charmander.SetMove(2, tackle);

        bulbasaur.SetMove(1, vineWhip);

        // TODO: Include more teams in defaults
        var starterTeam = new PokemonTeam("Starter Team", pikachu);
        starterTeam.AddPokemon(1, charmander);
        starterTeam.AddPokemon(2, bulbasaur);

        user.AddPokemon(pikachu);
        user.AddPokemon(charmander);
        user.AddPokemon(bulbasaur);

        user.AddPokemonTeam(starterTeam);

        user.AddMove(thunderbolt);
        user.AddMove(flamethrower);
        user.AddMove(vineWhip);
        user.AddMove(scratch);
        user.AddMove(tackle);
    }
}
