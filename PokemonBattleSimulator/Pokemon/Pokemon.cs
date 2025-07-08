using System;
using System.Text.Json.Serialization;

namespace PokemonBattleSimulator;

/// <summary>
/// Represents a defined Pokemon with its attributes and moves.
/// </summary>
internal record Pokemon
{
    public string Name { get; private set; }
    public int Level { get; private set; }
    public PokemonType FirstType { get; private set; }
    public PokemonType? SecondType { get; private set; }
    public int Health { get; private set; }
    public int Attack { get; private set; }
    public int Speed { get; private set; }
    public int SpecialAttack { get; private set; }
    public int Defense { get; private set; }
    public int SpecialDefense { get; private set; }

    /// <summary>
    /// Total number of moves a Pokemon can have.
    /// </summary>
    public static readonly int NumberOfMoves = 4;
    public Move[] Moves { get; private set; } = new Move[NumberOfMoves];
    public Pokemon(string name, int level, int health, int attack, int defense, int speed, int specialAttack, int specialDefense, Move firstMove, PokemonType firstType, PokemonType? secondType = null)
    {
        Name = name;
        Level = level;
        Health = health;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        SpecialAttack = specialAttack;
        SpecialDefense = specialDefense;
        FirstType = firstType;
        SecondType = secondType;
        Moves[0] = firstMove ?? throw new ArgumentNullException(nameof(firstMove), "First move cannot be null.");
    }

    [JsonConstructor] // Needed for deserialization
    public Pokemon(string name, int level, int health, int attack, int defense, int speed, int specialAttack, int specialDefense, Move[] moves, PokemonType firstType, PokemonType? secondType = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Pokemon name cannot be null.");
        Level = level;
        Health = health;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        SpecialAttack = specialAttack;
        SpecialDefense = specialDefense;
        FirstType = firstType;
        SecondType = secondType;
        if (moves == null || moves.Length != NumberOfMoves)
        {
            throw new ArgumentException($"Moves array must contain exactly {NumberOfMoves} moves.", nameof(moves));
        }

        for (int i = 0; i < NumberOfMoves; i++)
        {
            if (i == 0 && moves[i] == null)
            {
                throw new ArgumentNullException(nameof(moves), "First move cannot be null.");
            }
            Moves[i] = moves[i];
        }
    }

    /// <summary>
    /// Sets a move at the specified index for the Pokemon.
    /// </summary>
    /// <param name="index">Index of the move to set (0-3).</param>
    /// <param name="move">Move to set at the specified index.</param>
    /// <exception cref="ArgumentOutOfRangeException">Specified index is out of range (0-3).</exception>
    /// <exception cref="ArgumentNullException">First move is null.</exception>
    public void SetMove(int index, Move move)
    {
        if (index < 0 || index >= NumberOfMoves)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 3.");
        }
        if (move == null && index == 0)
        {
            throw new ArgumentNullException(nameof(move), "First move cannot be null.");
        }

        Moves[index] = move!;
    }
}
