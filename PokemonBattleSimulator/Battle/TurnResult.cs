using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Represents the result of a single turn in a Pokemon battle.
/// </summary>
internal enum TurnResult
{
    BattleOngoing, // The battle is still ongoing, no winner yet
    PokemonFainted, // A Pokemon has fainted this turn
}
