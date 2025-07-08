using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Interface for a console that prefixes messages with a specific string.
/// </summary>
internal interface IPrefixedConsole
{
    /// <summary>
    /// Writes a line to the console with the specified prefix.
    /// </summary>
    /// <param name="message">Message to write.</param>
    void WriteLine(string message);

    /// <summary>
    /// Writes a message to the console with the specified prefix without a newline.
    /// </summary>
    /// <param name="message">Message to write.</param>
    void Write(string message);

    /// <summary>
    /// Reads a line from the console, prompting with the specified prefix.
    /// </summary>
    /// <returns>A string containing the line read from the console, or null if input ended.</returns>
    string? ReadLine();
}

/// <summary>
/// A console implementation that prefixes all messages with a specified string.
/// </summary>
internal class PrefixedConsole : IPrefixedConsole
{
    private readonly string _prefix;    // From controller context

    /// <summary>
    /// Initializes a new instance of the <see cref="PrefixedConsole"/> class with a specified prefix.
    /// </summary>
    /// <param name="prefix">Prefix to prepend to all console messages.</param>
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
