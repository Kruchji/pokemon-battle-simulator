using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Interface for file operations, allowing for easier testing and mocking. 
/// </summary>
internal interface IFileWrapper
{
    void WriteAllText(string path, string content);
    bool Exists(string path);
    string ReadAllText(string path);
}

/// <summary>
/// Concrete implementation of IFileWrapper that uses System.IO for file operations.
/// </summary>
internal class FileWrapper : IFileWrapper
{
    public void WriteAllText(string path, string content) => File.WriteAllText(path, content);
    public bool Exists(string path) => File.Exists(path);
    public string ReadAllText(string path) => File.ReadAllText(path);
}
