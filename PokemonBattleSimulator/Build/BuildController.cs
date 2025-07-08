using System;
using System.Text.Json;

namespace PokemonBattleSimulator;

/// <summary>
/// Enables users to create and manage Pokemon, Moves, and Teams through a console interface.
/// </summary>
internal sealed class BuildController : IController
{
    private static readonly string _consolePrefix = "BuildMenu> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    private static readonly DataPersistence _dataPersistence = new DataPersistence(new FileWrapper());

    /// <summary>
    /// Runs the build menu, allowing users to create and manage their Pokemon, Moves, and Teams.
    /// </summary>
    /// <param name="user">User object containing the user's data.</param>
    public void Run(User user)
    {
        Console.WriteLine();
        _console.WriteLine("Welcome to the Build Menu!");
        _console.WriteLine("Here you can create and manage your Pokemon and Pokemon teams.\n");

        while (true)
        {
            _console.WriteLine("Type 'back' to return to the main menu.");
            _console.WriteLine("Type 'newM', 'newP', 'newT' to create a new Move, Pokemon or Team respectively.");
            _console.WriteLine("Type 'listM', 'listP', 'listT' to view paginated list of your Moves, Pokemon or Teams respectively.");
            _console.WriteLine("Type 'defaults' to load default data and 'clear' to clear all data.");
            _console.WriteLine("Type 'save' to save your data to a JSON and 'load' to load data from that file.\n");

            var userInput = _console.ReadLine()?.Trim().ToLower();

            switch (userInput)
            {
                case "back":
                    return;
                case "newm":
                    BuildManager.CreateMove(user);
                    break;
                case "newp":
                    BuildManager.CreatePokemon(user);
                    break;
                case "newt":
                    BuildManager.CreatePokemonTeam(user);
                    break;
                case "listm":
                    PaginatedLists.PaginatedListWithDeletion(user.Moves.Select(m => $"{m.Name} ({m.MoveType}, {m.Category}, ATK: {m.Power}, ACC: {m.Accuracy}, PP: {m.PP})").ToList(), "Moves", idx => user.RemoveMove(idx));
                    Console.WriteLine();
                    break;
                case "listp":
                    PaginatedLists.PaginatedListWithDeletion(user.PokemonList.Select(p => $"{p.Name} ({p.FirstType}" + (p.SecondType.HasValue ? $"/{p.SecondType}" : "") + $", LV: {p.Level})").ToList(), "Pokemon", idx => user.RemovePokemon(idx));
                    Console.WriteLine();
                    break;
                case "listt":
                    PaginatedLists.PaginatedListWithDeletion(user.PokemonTeams.Select(t => $"{t.Name} (Contains: " + string.Join(", ", t.PokemonList.Select(p => p?.Name ?? "empty")) + ")").ToList(), "Pokemon Teams", idx => user.RemovePokemonTeam(idx));
                    Console.WriteLine();
                    break;
                case "defaults":
                    _console.WriteLine("Loading default Pokemon and Moves...");
                    BuildManager.LoadDefaults(user);
                    _console.WriteLine("Default Pokemon and Moves loaded successfully.\n");
                    break;
                case "save":
                    _dataPersistence.SerializeUserData(user);
                    break;
                case "load":
                    _dataPersistence.DeserializeUserData(user);
                    break;
                case "clear":
                    _console.WriteLine("Clearing all data...");
                    user.ClearAllData();
                    _console.WriteLine("All data cleared successfully.\n");
                    break;
                default:
                    _console.WriteLine("Invalid command. Please type 'back' to return to the main menu.\n");
                    break;
            }
        }
    }
}
