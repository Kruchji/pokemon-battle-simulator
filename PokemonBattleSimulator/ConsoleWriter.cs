using System;

namespace PokemonBattleSimulator;

public interface IConsoleWriter
{
    void WriteLine(string message);
    void Write(string message);
}

public class ConsoleWriter : IConsoleWriter
{
    private readonly string _prefix;    // From controller context

    public ConsoleWriter(string prefix)
    {
        _prefix = prefix;
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(_prefix + message);
    }

    public void Write(string message)
    {
        Console.Write(_prefix + message);
    }
}
