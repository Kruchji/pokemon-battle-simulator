using System;

namespace PokemonBattleSimulator;

// Used for file operations to allow for easier testing and mocking
internal interface IFileWrapper
{
    void WriteAllText(string path, string content);
    bool Exists(string path);
    string ReadAllText(string path);
}

internal class FileWrapper : IFileWrapper
{
    public void WriteAllText(string path, string content) => File.WriteAllText(path, content);
    public bool Exists(string path) => File.Exists(path);
    public string ReadAllText(string path) => File.ReadAllText(path);
}
