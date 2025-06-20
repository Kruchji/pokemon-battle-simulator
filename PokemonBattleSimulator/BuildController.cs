﻿using System;

namespace PokemonBattleSimulator;

internal class BuildController : IController
{
    private static readonly string _consolePrefix = "BuildMenu> ";
    private readonly IConsoleWriter _console = new ConsoleWriter(_consolePrefix);
    public void Run(User user)
    {
        _console.WriteLine("Welcome to the Build Menu!");
        _console.WriteLine("Here you can create and manage your Pokemon and Pokemon teams.");

        _console.WriteLine("Type 'back' to return to the main menu.");
        while (true)
        {
            _console.Write("");
            var userInput = Console.ReadLine()?.Trim().ToLower();

            switch (userInput)
            {
                case "back":
                    return;
                case "add":
                    _console.WriteLine("Adding a new Pokemon to your team...");
                    // Logic to add a new Pokemon would go here
                    var examplePokemon = new Pokemon("Pikachu", 10, 10, 10, 10, 10, 10, 10, new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric);
                    user.AddPokemon(examplePokemon);
                    break;
                case "defaults":
                    _console.WriteLine("Loading default Pokemon and Moves...");
                    BuildManager.LoadDefaults(user);
                    _console.WriteLine("Default Pokemon and Moves loaded successfully.");
                    break;
                default:
                    _console.WriteLine("Invalid command. Please type 'back' to return to the main menu.");
                    break;
            }


        }
    }
}
