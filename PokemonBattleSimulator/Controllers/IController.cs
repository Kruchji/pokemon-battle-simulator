using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Interface for a controller that routes user in the console application.
/// </summary>
internal interface IController
{
    /// <summary>
    /// Runs the controller with the specified user. 
    /// </summary>
    /// <param name="user">User object containing user data.</param>
    void Run(User user);
}
