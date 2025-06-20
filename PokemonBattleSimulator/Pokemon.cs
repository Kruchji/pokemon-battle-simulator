using System;

namespace PokemonBattleSimulator;

public record Pokemon
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

    public static readonly int NumberOfMoves = 4;
    public Move[] Moves { get; private set; } = new Move[NumberOfMoves];    // TODO: Ensure that first move is always not null
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
        Moves[index] = move;
    }
}
