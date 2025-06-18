using System;

namespace PokemonBattleSimulator;

internal class BuildController : IController
{
    private static readonly string _consolePrefix = "BuildMenu> ";
    private readonly IConsoleWriter _console = new ConsoleWriter(_consolePrefix);
    public void Run()
    {
        _console.WriteLine("Welcome to the Build Menu!");
        _console.WriteLine("Here you can create and manage your Pokemon and Pokemon teams.");
        
        _console.WriteLine("Type 'back' to return to the main menu.");
        while (true)
        {
            _console.Write("");
            var userInput = Console.ReadLine()?.Trim().ToLower();
            if (userInput == "back")
            {
                break;
            }
            else
            {
                _console.WriteLine("Invalid command. Please type 'back' to return to the main menu.");
            }
        }
    }
}
