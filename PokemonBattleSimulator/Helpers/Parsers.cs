using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Provides utility methods for parsing strings into specific data types.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Tries to parse a string into an integer within a specified range.
    /// </summary>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns>Parser function that returns a tuple with a boolean indicating success and the parsed integer.</returns>
    public static Func<string, (bool, int)> TryParseIntInRange(int min, int max)
    {
        return input =>
        {
            bool parsed = int.TryParse(input, out int value);
            return (parsed && value >= min && value <= max, value);
        };
    }
}
