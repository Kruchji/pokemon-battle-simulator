using System;
using System.Reflection;

namespace PokemonBattleSimulator;

/// <summary>
/// Enables users to start battles between Pokemon or Pokemon Teams through a console interface.
/// </summary>
internal sealed class BattleController : IController
{
    private static readonly string _consolePrefix = "BattleMenu> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    private static readonly int _maxNumberOfBattles = 100000; // Maximum number of battles for simulation
    public void Run(User user)
    {
        Console.WriteLine();
        _console.WriteLine("Welcome to the Battle Menu!");
        _console.WriteLine("Here you can start a battle with your Pokemon teams.\n");

        while (true)
        {
            _console.WriteLine("Type 'back' to return to the main menu.");
            _console.WriteLine("Type 'battle' / 'battleT' to start a battle with two Pokemon / Pokemon Teams.");
            _console.WriteLine("Type 'battleMany' / 'battleTMany' to simulate many battles with two Pokemon / Pokemon Teams.\n");

            var userInput = _console.ReadLine()?.Trim().ToLower();

            switch (userInput)
            {
                case "back":
                    return;

                // Simple one-on-one battle
                case "battle":
                    _console.WriteLine("Setting up a battle with your Pokemon...");

                    var firstPokemon = BattleManager.SelectPokemonWithStrategy(user.PokemonList, "First");
                    if (firstPokemon == null) break;

                    var secondPokemon = BattleManager.SelectPokemonWithStrategy(user.PokemonList, "Second");
                    if (secondPokemon == null) break;

                    _console.WriteLine($"Starting battle between {firstPokemon.Name} and {secondPokemon.Name}...");
                    Battle.SimulateBattle(firstPokemon, secondPokemon, new PrefixedConsole(Battle.ConsolePrefix));
                    break;

                // Simulate many battles between two Pokemon
                case "battlemany":
                    _console.WriteLine("Setting up many battles with your Pokemon...");

                    int? battleCount = Prompts.PromptUntilValidInt("Enter the number of battles to simulate:", Parsers.TryParseIntInRange(1, _maxNumberOfBattles), "Please enter a valid number between 1 and 100000.");
                    if (battleCount == null) break;

                    var firstManyPokemon = BattleManager.SelectPokemonWithStrategy(user.PokemonList, "First");
                    if (firstManyPokemon == null) break;

                    var secondManyPokemon = BattleManager.SelectPokemonWithStrategy(user.PokemonList, "Second");
                    if (secondManyPokemon == null) break;

                    _console.WriteLine($"Starting {battleCount} battles between {firstManyPokemon.Name} and {secondManyPokemon.Name}...");

                    var (firstWins, secondWins) = Battle.SimulateManyBattles(firstManyPokemon, secondManyPokemon, (int)battleCount);
                    _console.WriteLine($"After {battleCount} battles: {firstManyPokemon.Name} won {firstWins} times, {secondManyPokemon.Name} won {secondWins} times.\n");

                    break;

                // Classic Pokemon team battle
                case "battlet":
                    _console.WriteLine("Setting up a team battle with your Pokemon teams...");

                    var firstTeam = BattleManager.SelectTeamWithStrategies(user.PokemonTeams, "First");
                    if (firstTeam == null) break;

                    var secondTeam = BattleManager.SelectTeamWithStrategies(user.PokemonTeams, "Second");
                    if (secondTeam == null) break;

                    _console.WriteLine($"Starting team battle between {firstTeam.Name} and {secondTeam.Name}...");
                    Battle.SimulateTeamBattle(firstTeam, secondTeam, new PrefixedConsole(Battle.ConsolePrefix));
                    break;

                // Simulate many team battles between two Pokemon teams
                case "battletmany":
                    _console.WriteLine("Setting up many team battles with your Pokemon teams...");

                    int? teamBattleCount = Prompts.PromptUntilValidInt("Enter the number of battles to simulate:", Parsers.TryParseIntInRange(1, _maxNumberOfBattles), "Please enter a valid number between 1 and 100000.");
                    if (teamBattleCount == null) break;

                    var firstManyTeam = BattleManager.SelectTeamWithStrategies(user.PokemonTeams, "First");
                    if (firstManyTeam == null) break;

                    var secondManyTeam = BattleManager.SelectTeamWithStrategies(user.PokemonTeams, "Second");
                    if (secondManyTeam == null) break;

                    _console.WriteLine($"Starting {teamBattleCount} team battles between {firstManyTeam.Name} and {secondManyTeam.Name}...");
                    var (firstTeamWins, secondTeamWins) = Battle.SimulateManyTeamBattles(firstManyTeam, secondManyTeam, (int)teamBattleCount);
                    _console.WriteLine($"After {teamBattleCount} battles: {firstManyTeam.Name} won {firstTeamWins} times, {secondManyTeam.Name} won {secondTeamWins} times.\n");
                    break;
                default:
                    _console.WriteLine("Invalid command. Please type 'back' to return to the main menu.\n");
                    break;
            }
        }
    }
}
