using System;

namespace PokemonBattleSimulator;

public interface IPrefixedConsole
{
    void WriteLine(string message);
    void Write(string message);
    string? ReadLine();
}

public class PrefixedConsole : IPrefixedConsole
{
    private readonly string _prefix;    // From controller context

    public PrefixedConsole(string prefix)
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

    public string? ReadLine()
    {
        Console.Write(_prefix);
        return Console.ReadLine();
    }
}
