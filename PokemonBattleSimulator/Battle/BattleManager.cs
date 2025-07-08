using System;
using System.Reflection;

namespace PokemonBattleSimulator;

/// <summary>
/// Provides methods to manage battles, including selecting Pokémon and teams with strategies.
/// </summary>
internal class BattleManager
{
    private static readonly string _consolePrefix = "BattleManager> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);

    /// <summary>
    /// Allows the user to select a Pokémon from a list of available Pokémon and assign an AI strategy to it.
    /// </summary>
    /// <param name="available">List of available Pokémon.</param>
    /// <param name="label">Label for the Pokémon selection (e.g., "First", "Second").</param>
    /// <returns>BattlePokemon instance with the selected Pokémon and assigned AI strategy, or null if selection was cancelled.</returns>
    public static BattlePokemon? SelectPokemonWithStrategy(List<Pokemon> available, string label)
    {
        _console.WriteLine($"Select {label} Pokémon:");
        var index = PaginatedLists.PaginatedListWithSelection(available.Select(p => p.Name).ToList(), $"{label} Pokémon");
        if (index == null) return null;

        var aiMethods = StrategyManager.GetStrategyMethods(typeof(AIStrategies), typeof(AIStrategy));
        _console.WriteLine($"Select AI strategy for {label} Pokémon:");
        var strat = StrategyManager.SelectStrategyMethod(aiMethods, $"{label} AI Strategy");
        if (strat == null) return null;

        return new BattlePokemon(
            available[index.Value],
            StrategyManager.CreateAIStrategyDelegate(strat)
        );
    }

    /// <summary>
    /// Allows the user to select a Pokémon team from a list of available teams and assign AI strategies to it.
    /// </summary>
    /// <param name="available">List of available Pokémon teams.</param>
    /// <param name="label">Label for the team selection (e.g., "First", "Second").</param>
    /// <returns>BattlePokemonTeam instance with the selected team and assigned AI strategies, or null if selection was cancelled.</returns>
    public static BattlePokemonTeam? SelectTeamWithStrategies(List<PokemonTeam> available, string label)
    {
        _console.WriteLine($"Select {label} Team:");
        var index = PaginatedLists.PaginatedListWithSelection(available.Select(t => t.Name).ToList(), $"{label} Team");
        if (index == null) return null;

        var moveStrats = StrategyManager.GetStrategyMethods(typeof(AIStrategies), typeof(AIStrategy));
        var teamStrats = StrategyManager.GetStrategyMethods(typeof(AITeamStrategies), typeof(AITeamStrategy));

        _console.WriteLine($"Select single Pokemon move strategy for {label} Team:");
        var moveStrat = StrategyManager.SelectStrategyMethod(moveStrats, $"{label} Move Strategy");
        if (moveStrat == null) return null;

        _console.WriteLine($"Select team strategy for {label} Team:");
        var teamStrat = StrategyManager.SelectStrategyMethod(teamStrats, $"{label} Team Strategy");
        if (teamStrat == null) return null;

        return new BattlePokemonTeam(
            available[index.Value],
            StrategyManager.CreateAIStrategyDelegate(moveStrat),
            StrategyManager.CreateAITeamStrategyDelegate(teamStrat)
        );
    }
}
