using System;

namespace PokemonBattleSimulator;

// Created on battle start, tracks pokemon's current state in battle
internal class BattlePokemon
{
    public Pokemon pokemon;
    public int CurrentHealth { get; private set; }
    public bool Fainted { get; private set; }

    public BattlePokemon(Pokemon pokemon)
    {
        this.pokemon = pokemon ?? throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
        CurrentHealth = pokemon.Health;
        Fainted = false;
    }
}
