using System;

namespace PokemonBattleSimulator;

public record Move
{
    public string Name { get; private set; }
    public int Power { get; private set; }
    public int Accuracy { get; private set; }   // TODO: check range, maybe change to float or double?
    public PokemonType MoveType { get; private set; }
    public int PP { get; private set; }
    public MoveCategory Category { get; private set; }

    public Move(string name, int power, int accuracy, int pp, PokemonType type, MoveCategory category)
    {
        Name = name;
        Power = power;
        Accuracy = accuracy;
        MoveType = type;
        PP = pp;
        Category = category;
    }
}
