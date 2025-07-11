﻿namespace PokemonBattleSimulator;

/// <summary>
/// Entry point for the Pokemon Battle Simulator application.
/// </summary>
internal static class Program
{
    static void Main(string[] args)
    {
        // Create a new user storing all Moves, Pokemon and Teams
        var user = new User();

        // Launch main menu
        var mainController = new MainController();
        mainController.Run(user);
    }
}
