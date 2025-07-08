using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Wraper for a Pokemon in battle, tracking its current health, battle moves, and AI strategy.
/// </summary>
internal class BattlePokemon
{
    /// <summary>
    /// Pokemon being wrapped in the BattlePokemon.
    /// </summary>
    public Pokemon Pokemon;

    /// <summary>
    /// Current health of the Pokemon in battle.
    /// </summary>
    public int CurrentHealth { get; private set; }

    /// <summary>
    /// Indicates whether the Pokemon has already fainted in battle.
    /// </summary>
    public bool Fainted { get; private set; }

    /// <summary>
    /// AI strategy to determine the next move in battle.
    /// </summary>
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

    // BattleMoves track current pp for each move
    public BattleMove[] BattleMoves { get; private set; } = new BattleMove[Pokemon.NumberOfMoves];

    // Move to use if all other moves have no PP left
    public static BattleMove FallbackMove = new FallbackMove();

    /// <summary>
    /// Initializes a new instance of the BattlePokemon class with a Pokemon and an AI strategy.
    /// </summary>
    /// <param name="pokemon">Pokemon to wrap in the BattlePokemon.</param>
    /// <param name="aiStrategy">AI strategy to determine the next move.</param>
    /// <exception cref="ArgumentNullException">Pokemon or AI strategy is null.</exception>
    public BattlePokemon(Pokemon pokemon, AIStrategy aiStrategy)
    {
        this.Pokemon = pokemon ?? throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
        CurrentHealth = pokemon.Health;
        Fainted = false;
        _aiStrategy = aiStrategy ?? throw new ArgumentNullException(nameof(aiStrategy), "AI strategy cannot be null.");

        for (int i = 0; i < Pokemon.NumberOfMoves; i++)
        {
            var move = Pokemon.Moves[i];
            BattleMoves[i] = new BattleMove(move); // Create a new BattleMove instance for each move
        }
    }

    /// <summary>
    /// Initializes a new instance of the BattlePokemon class by cloning another BattlePokemon.
    /// </summary>
    /// <param name="other">BattlePokemon to clone.</param>
    /// <exception cref="ArgumentNullException">BattlePokemon to clone is null.</exception>
    public BattlePokemon(BattlePokemon other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other), "Other BattlePokemon cannot be null.");
        Pokemon = other.Pokemon;
        CurrentHealth = other.CurrentHealth;
        Fainted = other.Fainted;
        _aiStrategy = other._aiStrategy; // Use the same AI strategy

        // Clone the BattleMoves array
        BattleMoves = new BattleMove[Pokemon.NumberOfMoves];
        for (int i = 0; i < Pokemon.NumberOfMoves; i++)
        {
            BattleMoves[i] = new BattleMove(other.BattleMoves[i]); // Create a new BattleMove instance for each move
        }
    }

    /// <summary>
    /// Applies damage to the Pokemon, reducing its current health.
    /// </summary>
    /// <param name="damage">Damage to apply to the Pokemon.</param>
    /// <exception cref="ArgumentOutOfRangeException">Damage is negative.</exception>
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

    /// <summary>
    /// Determines the next move to use against the opponent using the AI strategy.
    /// </summary>
    /// <param name="opponent">Opponent Pokemon in battle.</param>
    /// <returns>BattleMove to use against the opponent.</returns>
    /// <exception cref="ArgumentNullException">Opponent is null.</exception>
    public BattleMove GetNextMove(BattlePokemon opponent)
    {
        if (opponent == null) throw new ArgumentNullException(nameof(opponent), "Opponent cannot be null.");

        // If all moves have no PP left, use fallback move
        if (BattleMoves.All(battleMove => battleMove.CurrentPP <= 0))
        {
            return FallbackMove;
        }

        // Use the AI strategy to determine the next move
        var selectedMove = _aiStrategy(this, opponent);

        return selectedMove;
    }
}
