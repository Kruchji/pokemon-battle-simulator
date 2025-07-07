using System;

namespace PokemonBattleSimulator;

internal static class BuildManager
{
    private static readonly string _consolePrefix = "BuildManager> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);

    public static void CreateMove(User user)
    {
        _console.WriteLine("Let's create a new Pokemon move.");
        _console.WriteLine($"You can type '{Prompts.AbortCommand}' at any time to cancel the process.\n");

        string name = Prompts.PromptUntilValid("Enter move name:", s => !string.IsNullOrWhiteSpace(s), "Name cannot be empty.");
        if (name == null) return;

        int? power = Prompts.PromptUntilValidInt("Enter move power (0-250):", Parsers.TryParseIntInRange(0, 250), "Power must be a number between 0 and 250.");
        if (power == null) return;

        int? accuracy = Prompts.PromptUntilValidInt("Enter move accuracy (1-100):", Parsers.TryParseIntInRange(1, 100), "Accuracy must be between 1 and 100.");
        if (accuracy == null) return;

        int? pp = Prompts.PromptUntilValidInt("Enter move PP (1-40):", Parsers.TryParseIntInRange(1, 40), "PP must be between 1 and 40.");
        if (pp == null) return;

        PokemonType? moveType = Prompts.PromptEnum<PokemonType>("Enter move type (e.g., Fire, Water):");
        if (moveType == null) return;

        MoveCategory? category = Prompts.PromptEnum<MoveCategory>("Enter move category (e.g., Physical, Special):");
        if (category == null) return;

        var move = new Move(name, (int)power, (int)accuracy, (int)pp, (PokemonType)moveType, (MoveCategory)category);
        user.AddMove(move);

        Console.WriteLine();
        _console.WriteLine($"Move '{name}' created successfully!\n");
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
        _console.WriteLine($"You can type '{Prompts.AbortCommand}' at any time to cancel the process.\n");

        string name = Prompts.PromptUntilValid("Enter Pokémon name:", s => !string.IsNullOrWhiteSpace(s), "Name cannot be empty.");
        if (name == null) return;

        // Define stats

        int? level = Prompts.PromptUntilValidInt("Enter level (1-100):", Parsers.TryParseIntInRange(1, 100), "Level must be between 1 and 100.");
        if (level == null) return;

        int? health = Prompts.PromptUntilValidInt("Enter health (1-999):", Parsers.TryParseIntInRange(1, 999), "Health must be between 1 and 999.");
        if (health == null) return;

        int? attack = Prompts.PromptUntilValidInt("Enter attack (1-999):", Parsers.TryParseIntInRange(1, 999), "Attack must be between 1 and 999.");
        if (attack == null) return;

        int? defense = Prompts.PromptUntilValidInt("Enter defense (1-999):", Parsers.TryParseIntInRange(1, 999), "Defense must be between 1 and 999.");
        if (defense == null) return;

        int? speed = Prompts.PromptUntilValidInt("Enter speed (1-999):", Parsers.TryParseIntInRange(1, 999), "Speed must be between 1 and 999.");
        if (speed == null) return;

        int? spAttack = Prompts.PromptUntilValidInt("Enter special attack (1-999):", Parsers.TryParseIntInRange(1, 999), "Special Attack must be between 1 and 999.");
        if (spAttack == null) return;

        int? spDefense = Prompts.PromptUntilValidInt("Enter special defense (1-999):", Parsers.TryParseIntInRange(1, 999), "Special Defense must be between 1 and 999.");
        if (spDefense == null) return;

        // Define types

        PokemonType? firstType = Prompts.PromptEnum<PokemonType>("Enter first type:");
        if (firstType == null) return;

        (PokemonType? secondType, bool emptySecondType) = Prompts.PromptEnumOptional<PokemonType>("Enter second type (or press Enter to skip):");
        if (secondType == null && !emptySecondType) return;

        // Move selection

        Console.WriteLine();
        _console.WriteLine("Select at least one move (up to 4):");
        var selectedMoves = Prompts.PromptMoveSelection(user);
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

        user.AddPokemon(pokemon);

        Console.WriteLine();
        _console.WriteLine($"Pokémon '{name}' created successfully!\n");
    }

    public static void CreatePokemonTeam(User user)
    {
        // Guides user to create a new Pokémon team
        if (user.PokemonList.Count == 0)
        {
            _console.WriteLine("You must create at least one Pokémon before creating a team.");
            return;
        }

        _console.WriteLine("Let's create a new Pokémon team.");
        _console.WriteLine($"You can type '{Prompts.AbortCommand}' at any time to cancel the process.\n");

        string teamName = Prompts.PromptUntilValid("Enter team name:", s => !string.IsNullOrWhiteSpace(s), "Name cannot be empty.");
        if (teamName == null) return;

        var selectedPokemon = Prompts.PromptPokemonSelection(user);
        if (selectedPokemon == null) return;

        // Create team with name and Pokemon
        var pokemonTeam = new PokemonTeam(teamName, selectedPokemon[0]);
        for (int i = 1; i < selectedPokemon.Count; i++)
        {
            pokemonTeam.AddPokemon(i, selectedPokemon[i]);
        }

        // Add team to the user
        user.AddPokemonTeam(pokemonTeam);

        Console.WriteLine();
        _console.WriteLine($"Pokémon team '{teamName}' created successfully!\n");
    }

    public static void LoadDefaults(User user)
    {
        user.ClearAllData();

        // Create some default Pokemon and Moves
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

        // Create some default teams
        var starterTeam = new PokemonTeam("Starter Team", pikachu);
        starterTeam.AddPokemon(1, charmander);
        starterTeam.AddPokemon(2, bulbasaur);

        var bulbasaurTeam = new PokemonTeam("Bulbasaur Team", bulbasaur);
        bulbasaurTeam.AddPokemon(1, bulbasaur);
        bulbasaurTeam.AddPokemon(2, bulbasaur);
        bulbasaurTeam.AddPokemon(3, bulbasaur);
        bulbasaurTeam.AddPokemon(4, bulbasaur);
        bulbasaurTeam.AddPokemon(5, bulbasaur);

        var charmanderTeam = new PokemonTeam("Charmander Team", charmander);
        charmanderTeam.AddPokemon(1, charmander);
        charmanderTeam.AddPokemon(2, charmander);
        charmanderTeam.AddPokemon(3, charmander);
        charmanderTeam.AddPokemon(4, charmander);
        charmanderTeam.AddPokemon(5, charmander);

        user.AddPokemon(pikachu);
        user.AddPokemon(charmander);
        user.AddPokemon(bulbasaur);

        user.AddPokemonTeam(starterTeam);
        user.AddPokemonTeam(bulbasaurTeam);
        user.AddPokemonTeam(charmanderTeam);

        user.AddMove(thunderbolt);
        user.AddMove(flamethrower);
        user.AddMove(vineWhip);
        user.AddMove(scratch);
        user.AddMove(tackle);
    }
}
