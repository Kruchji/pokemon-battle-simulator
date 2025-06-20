using System;

namespace PokemonBattleSimulator;

internal class BattleController : IController
{
    private static readonly string _consolePrefix = "BattleMenu> ";
    private readonly IConsoleWriter _console = new ConsoleWriter(_consolePrefix);
    public void Run(User user)
    {
        _console.WriteLine("Welcome to the Battle Menu!");
        _console.WriteLine("Here you can start a battle with your Pokemon teams.");

        _console.WriteLine("Type 'back' to return to the main menu.");
        while (true)
        {
            _console.Write("");
            var userInput = Console.ReadLine()?.Trim().ToLower();

            switch (userInput)
            {
                case "back":
                    return;
                case "print":
                    _console.WriteLine("Printing all user Pokemon...");
                    foreach (var pokemon in user.PokemonList)
                    {
                        _console.WriteLine(pokemon.ToString());
                    }
                    break;
                case "battle":
                    _console.WriteLine("Starting a battle with your Pokemon teams...");

                    // TODO: Selection logic
                    var firstPokemon = new BattlePokemon(user.PokemonList[0], AIStrategies.RandomMove);
                    var secondPokemon = new BattlePokemon(user.PokemonList[0], AIStrategies.BestOverallMove);
                    Battle.SimulateBattle(firstPokemon, secondPokemon, _console);
                    break;
                default:
                    _console.WriteLine("Invalid command. Please type 'back' to return to the main menu.");
                    break;
            }
        }
    }
}
