using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Represents a fallback move in the simulator, used when all other moves have no PP left. Has infinite PP.
/// </summary>
internal sealed class FallbackMove : BattleMove
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FallbackMove"/> class with predefined properties.
    /// </summary>
    public FallbackMove() : base(new Move("Struggle", 50, 100, 1, PokemonType.Normal, MoveCategory.Physical))
    { }

    /// <summary>
    /// Uses the fallback move. This move has infinite PP, so it does not consume any PP when used.
    /// </summary>
    public override void UseMove()
    {
        // Does nothing, as this move has infinite PP
    }
}
