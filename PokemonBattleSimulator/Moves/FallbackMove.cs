using System;

namespace PokemonBattleSimulator;

// Fallback move with infinite PP, used when all other moves have no PP left
internal sealed class FallbackMove : BattleMove
{
    public FallbackMove() : base(new Move("Struggle", 50, 100, 1, PokemonType.Normal, MoveCategory.Physical))
    { }

    public override void UseMove()
    {
        // Does nothing, as this move has infinite PP
    }
}
