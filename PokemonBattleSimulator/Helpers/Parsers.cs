using System;

namespace PokemonBattleSimulator;

public static class Parsers
{
    public static Func<string, (bool, int)> TryParseIntInRange(int min, int max)
    {
        return input =>
        {
            bool parsed = int.TryParse(input, out int value);
            return (parsed && value >= min && value <= max, value);
        };
    }
}
