using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Wrapper for a Move used in battles, managing its current PP (Power Points).
/// </summary>
internal class BattleMove
{
    public int CurrentPP { get; private set; }
    public Move Move { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BattleMove"/> class from a specified move.
    /// </summary>
    /// <param name="move">Move to wrap in the BattleMove.</param>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="BattleMove"/> class by copying another BattleMove.
    /// </summary>
    /// <param name="other">BattleMove to copy from.</param>
    /// <exception cref="ArgumentNullException">BattleMove to copy from is null.</exception>
    public BattleMove(BattleMove other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other), "Other BattleMove cannot be null.");
        Move = other.Move;
        CurrentPP = other.CurrentPP; // Copy the current PP
    }

    /// <summary>
    /// Uses the move, decreasing its current PP by 1.
    /// </summary>
    /// <exception cref="InvalidOperationException">Move has no current PP left.</exception>
    public virtual void UseMove()
    {
        if (CurrentPP <= 0)
        {
            throw new InvalidOperationException("Cannot use move, no PP left.");
        }
        CurrentPP--; // Decrease PP when the move is used
    }
}
