using System;

namespace PokemonBattleSimulator;

// Created on battle start, tracks pokemon's current state in battle
public class BattlePokemon
{
    public Pokemon Pokemon;
    public int CurrentHealth { get; private set; }
    public bool Fainted { get; private set; }
    private readonly AIStrategy _aiStrategy;

    // References to the original Pokemon object
    public string Name => Pokemon.Name;
    public int Level => Pokemon.Level;
    public PokemonType FirstType => Pokemon.FirstType;
    public PokemonType? SecondType => Pokemon.SecondType;
    public int Health => Pokemon.Health;
    public int Attack => Pokemon.Attack;
    public int Defense => Pokemon.Defense;
    public int Speed => Pokemon.Speed;
    public int SpecialAttack => Pokemon.SpecialAttack;
    public int SpecialDefense => Pokemon.SpecialDefense;


    public BattlePokemon(Pokemon pokemon, AIStrategy aiStrategy)
    {
        this.Pokemon = pokemon ?? throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
        CurrentHealth = pokemon.Health;
        Fainted = false;
        _aiStrategy = aiStrategy ?? throw new ArgumentNullException(nameof(aiStrategy), "AI strategy cannot be null.");
    }

    // Constructor from existing BattlePokemon, used for cloning
    public BattlePokemon(BattlePokemon other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other), "Other BattlePokemon cannot be null.");
        Pokemon = other.Pokemon;
        CurrentHealth = other.CurrentHealth;
        Fainted = other.Fainted;
        _aiStrategy = other._aiStrategy; // Use the same AI strategy
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");

        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Fainted = true;
        }
    }

    public Move GetNextMove(BattlePokemon opponent)
    {
        if (opponent == null) throw new ArgumentNullException(nameof(opponent), "Opponent cannot be null.");

        // Use the AI strategy to determine the next move
        return _aiStrategy(this, opponent);
    }
}
