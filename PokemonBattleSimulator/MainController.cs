using System;

namespace PokemonBattleSimulator;

internal class MainController : IController
{
    private static readonly string _consolePrefix = "MainMenu> ";
    private readonly IConsoleWriter _console = new ConsoleWriter(_consolePrefix);
    public void Run(User user)
    {
        _console.WriteLine("Welcome to the Pokemon Battle Simulator!");
        _console.WriteLine("Type 'build' to create a Pokemon team, 'battle' to start a battle, or 'exit' to quit the application.");
        while (true)
        {
            _console.Write("");
            var userInput = Console.ReadLine()?.Trim().ToLower();

            switch (userInput)
            {
                case "build":
                    var buildController = new BuildController();
                    buildController.Run(user);
                    break;
                case "battle":
                    var battleController = new BattleController();
                    battleController.Run(user);
                    break;
                case "exit":
                    _console.WriteLine("Exiting the application. Goodbye!");
                    return;
                default:
                    _console.WriteLine("Invalid command. Please type 'build', 'battle', or 'exit'.");
                    break;
            }
        }
    }
}
