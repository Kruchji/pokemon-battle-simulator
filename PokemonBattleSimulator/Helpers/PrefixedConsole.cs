using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Interface for a console that prefixes messages with a specific string.
/// </summary>
internal interface IPrefixedConsole
{
    void WriteLine(string message);
    void Write(string message);
    string? ReadLine();
}

/// <summary>
/// A console implementation that prefixes all messages with a specified string.
/// </summary>
internal class PrefixedConsole : IPrefixedConsole
{
    private readonly string _prefix;    // From controller context

    public PrefixedConsole(string prefix)
    {
        _prefix = prefix;
    }

    /// <summary>
    /// Writes a line to the console with the specified prefix.
    /// </summary>
    /// <param name="message">Message to write.</param>
    public void WriteLine(string message)
    {
        Console.WriteLine(_prefix + message);
    }

    /// <summary>
    /// Writes a message to the console with the specified prefix without a newline.
    /// </summary>
    /// <param name="message">Message to write.</param>
    public void Write(string message)
    {
        Console.Write(_prefix + message);
    }

    /// <summary>
    /// Reads a line from the console, prompting with the specified prefix.
    /// </summary>
    /// <returns>A string containing the line read from the console, or null if input ended.</returns>
    public string? ReadLine()
    {
        Console.Write(_prefix);
        return Console.ReadLine();
    }
}
