using System;

namespace PokemonBattleSimulator;

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
    public Pokemon(string name, int level, int health, int attack, int defense, int speed, int specialAttack, int specialDefense, PokemonType firstType, PokemonType? secondType = null)
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
    }
}
