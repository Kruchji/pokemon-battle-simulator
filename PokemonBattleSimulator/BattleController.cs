using System;
using System.Reflection;

namespace PokemonBattleSimulator;

internal class BattleController : IController
{
    private static readonly string _consolePrefix = "BattleMenu> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    public void Run(User user)
    {
        Console.WriteLine();
        _console.WriteLine("Welcome to the Battle Menu!");
        _console.WriteLine("Here you can start a battle with your Pokemon teams.\n");

        while (true)
        {
            _console.WriteLine("Type 'back' to return to the main menu.");
            _console.WriteLine("Type 'battle' to start a battle with two Pokemon.");
            _console.WriteLine("Type 'battleMany' to simulate many battles with two Pokemon.");
            _console.WriteLine("Type 'battleTeam' to start a team battle with two Pokemon teams.");
            _console.WriteLine("Type 'battleTeamMany' to simulate many team battles with two Pokemon teams.\n");

            var userInput = _console.ReadLine()?.Trim().ToLower();

            // TODO: Move parts to separate methods
            switch (userInput)
            {
                case "back":
                    return;
                case "battle":
                    _console.WriteLine("Setting up a battle with your Pokemon...");

                    var firstPokemon = SelectPokemonWithStrategy(user.PokemonList, "First");
                    if (firstPokemon == null) break;

                    var secondPokemon = SelectPokemonWithStrategy(user.PokemonList, "Second");
                    if (secondPokemon == null) break;

                    _console.WriteLine($"Starting battle between {firstPokemon.Name} and {secondPokemon.Name}...");
                    Battle.SimulateBattle(firstPokemon, secondPokemon, _console);
                    break;
                case "battlemany":
                    _console.WriteLine("Setting up many battles with your Pokemon...");

                    // TODO: Test limits
                    int battleCount = PromptUntilValid("Enter the number of battles to simulate:", TryParseIntInRange(1, 100000), "Please enter a valid number between 1 and 100000.");

                    var firstManyPokemon = SelectPokemonWithStrategy(user.PokemonList, "First");
                    if (firstManyPokemon == null) break;

                    var secondManyPokemon = SelectPokemonWithStrategy(user.PokemonList, "Second");
                    if (secondManyPokemon == null) break;

                    _console.WriteLine($"Starting {battleCount} battles between {firstManyPokemon.Name} and {secondManyPokemon.Name}...");

                    var (firstWins, secondWins) = Battle.SimulateManyBattles(firstManyPokemon, secondManyPokemon, battleCount);
                    _console.WriteLine($"After {battleCount} battles: {firstManyPokemon.Name} won {firstWins} times, {secondManyPokemon.Name} won {secondWins} times.");

                    break;
                case "battleteam":
                    _console.WriteLine("Setting up a team battle with your Pokemon teams...");

                    var firstTeam = SelectTeamWithStrategies(user.PokemonTeams, "First");
                    if (firstTeam == null) break;

                    var secondTeam = SelectTeamWithStrategies(user.PokemonTeams, "Second");
                    if (secondTeam == null) break;

                    _console.WriteLine($"Starting team battle between {firstTeam.Name} and {secondTeam.Name}...");
                    Battle.SimulateTeamBattle(firstTeam, secondTeam, _console);
                    break;
                case "battleteammany":
                    _console.WriteLine("Setting up many team battles with your Pokemon teams...");

                    int teamBattleCount = PromptUntilValid("Enter the number of battles to simulate:", TryParseIntInRange(1, 100000), "Please enter a valid number between 1 and 100000.");

                    var firstManyTeam = SelectTeamWithStrategies(user.PokemonTeams, "First");
                    if (firstManyTeam == null) break;

                    var secondManyTeam = SelectTeamWithStrategies(user.PokemonTeams, "Second");
                    if (secondManyTeam == null) break;

                    _console.WriteLine($"Starting {teamBattleCount} team battles between {firstManyTeam.Name} and {secondManyTeam.Name}...");
                    var (firstTeamWins, secondTeamWins) = Battle.SimulateManyTeamBattles(firstManyTeam, secondManyTeam, teamBattleCount);
                    _console.WriteLine($"After {teamBattleCount} battles: {firstManyTeam.Name} won {firstTeamWins} times, {secondManyTeam.Name} won {secondTeamWins} times.");
                    break;
                default:
                    _console.WriteLine("Invalid command. Please type 'back' to return to the main menu.\n");
                    break;
            }
        }
    }

    private BattlePokemon? SelectPokemonWithStrategy(List<Pokemon> available, string label)
    {
        _console.WriteLine($"Select {label} Pokémon:");
        var index = SelectFromPaginatedList(available.Select(p => p.Name).ToList(), $"{label} Pokémon");
        if (index == null) return null;

        var aiMethods = GetStrategyMethods(typeof(AIStrategies), typeof(AIStrategy));
        _console.WriteLine($"Select AI strategy for {label} Pokémon:");
        var strat = SelectStrategyMethod(aiMethods, $"{label} AI Strategy");
        if (strat == null) return null;

        return new BattlePokemon(
            available[index.Value],
            CreateAIStrategyDelegate(strat)
        );
    }

    private BattlePokemonTeam? SelectTeamWithStrategies(List<PokemonTeam> available, string label)
    {
        _console.WriteLine($"Select {label} Team:");
        var index = SelectFromPaginatedList(available.Select(t => t.Name).ToList(), $"{label} Team");
        if (index == null) return null;

        var moveStrats = GetStrategyMethods(typeof(AIStrategies), typeof(AIStrategy));
        var teamStrats = GetStrategyMethods(typeof(AITeamStrategies), typeof(AITeamStrategy));

        _console.WriteLine($"Select single Pokemon move strategy for {label} Team:");
        var moveStrat = SelectStrategyMethod(moveStrats, $"{label} Move Strategy");
        if (moveStrat == null) return null;

        _console.WriteLine($"Select team strategy for {label} Team:");
        var teamStrat = SelectStrategyMethod(teamStrats, $"{label} Team Strategy");
        if (teamStrat == null) return null;

        return new BattlePokemonTeam(
            available[index.Value],
            CreateAIStrategyDelegate(moveStrat),
            CreateAITeamStrategyDelegate(teamStrat)
        );
    }

    // TODO: Move helpers to separate class?

    // For int validation
    private static int PromptUntilValid(string prompt, Func<string, (bool success, int value)> parser, string errorMessage)
    {
        while (true)
        {
            _console.WriteLine(prompt);
            string? input = _console.ReadLine();
            var (success, value) = parser(input ?? "");
            if (success) return value;
            _console.WriteLine(errorMessage);
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

    private List<MethodInfo> GetStrategyMethods(Type strategyClass, Type delegateType)
    {
        return strategyClass
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(m =>
            {
                try
                {
                    Delegate.CreateDelegate(delegateType, m);
                    return true;    // Assignable to the delegate type
                }
                catch (ArgumentException)
                {
                    return false; // Not assignable to the delegate type
                }
            })
            .ToList();
    }

    private MethodInfo? SelectStrategyMethod(List<MethodInfo> methods, string title)
    {
        var names = methods.Select(m => m.Name).ToList();
        int? index = SelectFromPaginatedList(names, title);
        return index.HasValue ? methods[index.Value] : null;
    }

    private AIStrategy CreateAIStrategyDelegate(MethodInfo method)
    {
        return (AIStrategy)Delegate.CreateDelegate(typeof(AIStrategy), method);
    }

    private AITeamStrategy CreateAITeamStrategyDelegate(MethodInfo method)
    {
        return (AITeamStrategy)Delegate.CreateDelegate(typeof(AITeamStrategy), method);
    }

    private int? SelectFromPaginatedList(List<string> items, string title)
    {
        const int pageSize = 10;
        if (items.Count == 0)
        {
            _console.WriteLine($"No {title} found.");
            return null;
        }

        int page = 0;
        int totalPages = (int)Math.Ceiling((double)items.Count / pageSize);
        while (true)
        {
            _console.WriteLine("");
            _console.WriteLine($"--- {title} (Page {page + 1}/{totalPages}) ---");

            var pageItems = items.Skip(page * pageSize).Take(pageSize).ToList();
            for (int i = 0; i < pageItems.Count; i++)
            {
                _console.WriteLine($"{i + 1}. {pageItems[i]}");
            }

            _console.WriteLine("");
            _console.WriteLine("Type number to select or 'n' for next page, 'p' for previous, 'q' to cancel:");
            string? input = _console.ReadLine()?.Trim().ToLower();

            if (input == "q")
                return null;
            else if (input == "n" && (page + 1) * pageSize < items.Count)
                page++;
            else if (input == "p" && page > 0)
                page--;
            else if (int.TryParse(input, out int choice))
            {
                int absoluteIndex = page * pageSize + (choice - 1);
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
}
