using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonBattleSimulator;

internal static class DataPersistence
{
    private static readonly string _consolePrefix = "DataPersistence> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    private static readonly string _userDataFile = "user.json";
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

    public static void SerializeUserData(User user)
    {
        _console.WriteLine($"Saving user data to '{_userDataFile}'...");
        string json = JsonSerializer.Serialize(user, _jsonSerializerOptions);

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

    public static void DeserializeUserData(User user)
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
