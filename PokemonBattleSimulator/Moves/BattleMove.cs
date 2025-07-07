using System;

namespace PokemonBattleSimulator;

public class BattleMove
{
    public int CurrentPP { get; private set; }
    public Move Move { get; private set; }

    public BattleMove(Move move)
    {
        Move = move;

        if (move == null)
        {
            CurrentPP = 0; // If move is null, set PP to 0
        }
        else
        {
            CurrentPP = move.PP; // Initialize with full PP
        }
    }

    public BattleMove(BattleMove other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other), "Other BattleMove cannot be null.");
        Move = other.Move;
        CurrentPP = other.CurrentPP; // Copy the current PP
    }

    public virtual void UseMove()
    {
        if (CurrentPP <= 0)
        {
            throw new InvalidOperationException("Cannot use move, no PP left.");
        }
        CurrentPP--; // Decrease PP when the move is used
    }
}
