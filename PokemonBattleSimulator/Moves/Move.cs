using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Represents a defined Pokemon move.
/// </summary>
internal record Move
{
    public string Name { get; private set; }
    public int Power { get; private set; }
    public int Accuracy { get; private set; }
    public PokemonType MoveType { get; private set; }
    public int PP { get; private set; }
    public MoveCategory Category { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Move class with specified parameters.
    /// </summary>
    /// <param name="name">Name of the move.</param>
    /// <param name="power">Power of the move.</param>
    /// <param name="accuracy">Accuracy of the move.</param>
    /// <param name="pp">PP (Power Points) of the move.</param>
    /// <param name="moveType">Type of the move (e.g., Fire, Water).</param>
    /// <param name="category">Category of the move (e.g., Physical, Special).</param>
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
