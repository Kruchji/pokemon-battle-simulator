using System;

namespace PokemonBattleSimulator;

internal record Move
{
    public string Name { get; private set; }
    public int Power { get; private set; }
    public int Accuracy { get; private set; }
    public PokemonType MoveType { get; private set; }
    public int PP { get; private set; }
    public MoveCategory Category { get; private set; }

    public Move(string name, int power, int accuracy, int pp, PokemonType moveType, MoveCategory category)
    {
        Name = name;
        Power = power;
        Accuracy = accuracy;
        MoveType = moveType;
        PP = pp;
        Category = category;
    }
}
