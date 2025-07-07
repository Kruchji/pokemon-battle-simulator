using System;
using System.Text.Json;

namespace PokemonBattleSimulator;

internal class BuildController : IController
{
    private static readonly string _consolePrefix = "BuildMenu> ";
    private readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    private static readonly string _userDataFile = "user.json";

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
            _console.WriteLine("Type 'defaults' to load default Pokemon and Moves.\n");

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
                    PaginateList(user.Moves.Select(m => $"{m.Name} ({m.MoveType}, {m.Category}, ATK: {m.Power}, ACC: {m.Accuracy}, PP: {m.PP})").ToList(), "Moves", idx => user.RemoveMove(idx));
                    break;
                case "listp":
                    PaginateList(user.PokemonList.Select(p => $"{p.Name} ({p.FirstType}" + (p.SecondType.HasValue ? $"/{p.SecondType}" : "") + $", LV: {p.Level})").ToList(), "Pokemon", idx => user.RemovePokemon(idx));
                    break;
                case "listt":
                    PaginateList(user.PokemonTeams.Select(t => $"{t.Name} (Contains: " + string.Join(", ", t.PokemonList.Select(p => p?.Name ?? "empty")) + ")").ToList(), "Pokemon Teams", idx => user.RemovePokemonTeam(idx));
                    break;
                case "defaults":
                    _console.WriteLine("Loading default Pokemon and Moves...");
                    BuildManager.LoadDefaults(user);
                    _console.WriteLine("Default Pokemon and Moves loaded successfully.");
                    break;
                case "save":
                    _console.WriteLine($"Saving user data to '{_userDataFile}'...");
                    SerializeUserData(user);
                    break;
                case "load":
                    DeserializeUserData(user);
                    break;
                case "clear":
                    _console.WriteLine("Clearing all data...");
                    user.ClearAllData();
                    break;
                default:
                    _console.WriteLine("Invalid command. Please type 'back' to return to the main menu.\n");
                    break;
            }
        }
    }

    // TODO: Move separately to different class
    private void PaginateList(List<string> items, string title, Action<int>? onDelete = null)
    {
        const int pageSize = 10;
        if (items.Count == 0)
        {
            _console.WriteLine($"No {title} found.");
            return;
        }

        int page = 0;
        int totalPages = (int)Math.Ceiling((double)items.Count / pageSize);
        while (true)
        {
            Console.WriteLine();
            _console.WriteLine($"--- {title} (Page {page + 1}/{totalPages}) ---");

            var pageItems = items.Skip(page * pageSize).Take(pageSize).ToList();
            for (int i = 0; i < pageItems.Count; i++)
            {
                _console.WriteLine($"{i + 1}. {pageItems[i]}");
            }

            Console.WriteLine();
            _console.WriteLine("Type 'n' for next page, 'p' for previous, 'q' to quit.");
            if (onDelete != null)
                _console.WriteLine("Type 'delete (number)' to remove an item.");
            string? input = _console.ReadLine()?.Trim().ToLower();
            if (input == "q") break;
            else if (input == "n" && (page + 1) * pageSize < items.Count)
                page++;
            else if (input == "p" && page > 0)
                page--;
            else if (input.StartsWith("delete") && onDelete != null)
            {
                var parts = input.Split(' ');
                if (parts.Length == 2 && int.TryParse(parts[1], out int index))
                {
                    int absoluteIndex = page * pageSize + index - 1;
                    if (absoluteIndex >= 0 && absoluteIndex < items.Count)
                    {
                        onDelete(absoluteIndex);
                        items.RemoveAt(absoluteIndex);
                        totalPages = (int)Math.Ceiling((double)items.Count / pageSize);
                        if (page >= totalPages) page = Math.Max(0, totalPages - 1);
                    }
                    else
                    {
                        _console.WriteLine("Invalid index.");
                    }
                }
                else
                {
                    _console.WriteLine("Invalid delete command. Use: delete {number}");
                }
            }
        }
    }

    private void SerializeUserData(User user)
    {
        string json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });

        try
        {
            File.WriteAllText(_userDataFile, json);
            _console.WriteLine($"User data saved successfully to '{_userDataFile}'.");
        }
        catch (IOException ex)
        {
            _console.WriteLine($"IO error while saving user data: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _console.WriteLine($"Permission error saving user data: {ex.Message}");
        }

    }

    private void DeserializeUserData(User user)
    {
        if (File.Exists(_userDataFile))
        {
            _console.WriteLine($"Loading user data from '{_userDataFile}'...");

            // Try to read from the file
            string jsonData;
            try
            {
                jsonData = File.ReadAllText(_userDataFile);
            }
            catch (IOException ex)
            {
                _console.WriteLine($"IO error while reading user data: {ex.Message}");
                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                _console.WriteLine($"Permission error reading user data: {ex.Message}");
                return;
            }

            User deserializedUser;
            try
            {
                deserializedUser = JsonSerializer.Deserialize<User>(jsonData)!;
            }
            catch (JsonException ex)
            {
                _console.WriteLine($"Invalid JSON format of data file: {ex.Message}");
                return;
            }

            if (deserializedUser != null)
            {
                user.CopyFrom(deserializedUser);
                _console.WriteLine("User data loaded successfully.");
            }
            else
            {
                _console.WriteLine("Failed to deserialize user data. Please check the file format.");
            }
        }
        else
        {
            _console.WriteLine($"No user data found. Check if '{_userDataFile}' exists in the current directory.");
        }
    }
}
