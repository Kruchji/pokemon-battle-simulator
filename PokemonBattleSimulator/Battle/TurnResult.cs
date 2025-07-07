using System;

namespace PokemonBattleSimulator;

public enum TurnResult
{
    BattleOngoing, // The battle is still ongoing, no winner yet
    PokemonFainted, // A Pokemon has fainted this turn
}
