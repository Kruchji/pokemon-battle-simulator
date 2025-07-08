using System;
using System.Text.Json;

namespace PokemonBattleSimulator;

/// <summary>
/// Handles serialization and deserialization of user data to and from a JSON file.
/// </summary>
internal class DataPersistence
{
    private static readonly string _consolePrefix = "DataPersistence> ";
    private static readonly IPrefixedConsole _console = new PrefixedConsole(_consolePrefix);
    private static readonly string _userDataFile = "user.json";
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
    private readonly IFileWrapper _fileWrapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataPersistence"/> class.
    /// </summary>
    /// <param name="fileWrapper">File wrapper for file operations.</param>
    /// <exception cref="ArgumentNullException">File wrapper is null.</exception>
    public DataPersistence(IFileWrapper fileWrapper)
    {
        _fileWrapper = fileWrapper ?? throw new ArgumentNullException(nameof(fileWrapper));
    }

    /// <summary>
    /// Serializes the user data to a JSON file.
    /// </summary>
    /// <param name="user">User object containing data to serialize.</param>
    public void SerializeUserData(User user)
    {
        _console.WriteLine($"Saving user data to '{_userDataFile}'...");
        string json = JsonSerializer.Serialize(user, _jsonSerializerOptions);

        // Try to write the serialized JSON to the file
        try
        {
            _fileWrapper.WriteAllText(_userDataFile, json);
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

    /// <summary>
    /// Deserializes user data from a JSON file into the provided User object.
    /// </summary>
    /// <param name="user">User object to populate with data from the file.</param>
    public void DeserializeUserData(User user)
    {
        if (_fileWrapper.Exists(_userDataFile))
        {
            _console.WriteLine($"Loading user data from '{_userDataFile}'...");

            // Try to read from the file
            string jsonData;
            try
            {
                jsonData = _fileWrapper.ReadAllText(_userDataFile);
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

            // Try to deserialize the JSON data into a User object
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

            // Copy the deserialized data into the provided User object
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
