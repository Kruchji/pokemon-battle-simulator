using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Main controller for the Pokemon Battle Simulator application. Routes user to the battle or build menus based on user input.
/// </summary>
internal sealed class MainController : IController
{
    private static readonly string _consolePrefix = "MainMenu> ";
    private readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    private bool _firstRun = true;
    public void Run(User user)
    {
        if (!_firstRun)
        {
            Console.WriteLine();
        }
        _firstRun = false;
        _console.WriteLine("Welcome to the Pokemon Battle Simulator!\n");

        while (true)
        {
            _console.WriteLine("Type 'build' manage your Moves and Pokemon, 'battle' to start a battle, or 'exit' to quit the application.\n");
            var userInput = _console.ReadLine()?.Trim().ToLower();

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
                    _console.WriteLine("Exiting the simulator. Goodbye!");
                    return;
                default:
                    _console.WriteLine("Invalid command. Please type 'build', 'battle', or 'exit'.\n");
                    break;
            }
        }
    }
}
