using System;

namespace PokemonBattleSimulator;

internal enum TurnResult
{
    BattleOngoing, // The battle is still ongoing, no winner yet
    PokemonFainted, // A Pokemon has fainted this turn
}
