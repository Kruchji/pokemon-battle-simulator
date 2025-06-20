using System;

namespace PokemonBattleSimulator;

// Store type table and have methods to calculate effectiveness
internal static class TypeCalculator
{
    // Stores effectiveness of types (Attacker, Defender)
    private static readonly double[,] _typeChart;

    private static readonly int _numTypes = Enum.GetValues(typeof(PokemonType)).Length;

    private static readonly double _normallyEffective = 1; // Neutral effectiveness
    private static readonly double _notVeryEffective = 0.5; // Not very effective
    private static readonly double _superEffective = 2; // Super effective
    private static readonly double _noEffect = 0; // No effect

    static TypeCalculator()
    {
        // Initialize the type chart with effectiveness values
        _typeChart = new double[_numTypes, _numTypes];

        // Fill in default effectiveness values
        for (int i = 0; i < _numTypes; i++)
        {
            for (int j = 0; j < _numTypes; j++)
            {
                _typeChart[i, j] = _normallyEffective; // Default effectiveness is neutral
            }
        }

        // Normal
        SetTypeChartValues(PokemonType.Normal,
            superEffective: [],
            notVeryEffective: [PokemonType.Rock, PokemonType.Steel],
            noEffect: [PokemonType.Ghost]);

        // Fire
        SetTypeChartValues(PokemonType.Fire,
            superEffective: [PokemonType.Grass, PokemonType.Ice, PokemonType.Bug, PokemonType.Steel],
            notVeryEffective: [PokemonType.Fire, PokemonType.Water, PokemonType.Rock, PokemonType.Dragon],
            noEffect: []);

        // Water
        SetTypeChartValues(PokemonType.Water,
            superEffective: [PokemonType.Fire, PokemonType.Ground, PokemonType.Rock],
            notVeryEffective: [PokemonType.Water, PokemonType.Grass, PokemonType.Dragon],
            noEffect: []);

        // Grass
        SetTypeChartValues(PokemonType.Grass,
            superEffective: [PokemonType.Water, PokemonType.Ground, PokemonType.Rock],
            notVeryEffective: [PokemonType.Fire, PokemonType.Grass, PokemonType.Poison, PokemonType.Flying, PokemonType.Bug, PokemonType.Dragon, PokemonType.Steel],
            noEffect: []);

        // Electric
        SetTypeChartValues(PokemonType.Electric,
            superEffective: [PokemonType.Water, PokemonType.Flying],
            notVeryEffective: [PokemonType.Grass, PokemonType.Electric, PokemonType.Dragon],
            noEffect: [PokemonType.Ground]);

        // Ice
        SetTypeChartValues(PokemonType.Ice,
            superEffective: [PokemonType.Grass, PokemonType.Ground, PokemonType.Flying, PokemonType.Dragon],
            notVeryEffective: [PokemonType.Fire, PokemonType.Water, PokemonType.Ice, PokemonType.Steel],
            noEffect: []);

        // Fighting
        SetTypeChartValues(PokemonType.Fighting,
            superEffective: [PokemonType.Normal, PokemonType.Ice, PokemonType.Rock, PokemonType.Dark, PokemonType.Steel],
            notVeryEffective: [PokemonType.Poison, PokemonType.Flying, PokemonType.Psychic, PokemonType.Bug, PokemonType.Fairy],
            noEffect: [PokemonType.Ghost]);

        // Poison
        SetTypeChartValues(PokemonType.Poison,
            superEffective: [PokemonType.Grass, PokemonType.Fairy],
            notVeryEffective: [PokemonType.Poison, PokemonType.Ground, PokemonType.Rock, PokemonType.Ghost],
            noEffect: []);

        // Ground
        SetTypeChartValues(PokemonType.Ground,
            superEffective: [PokemonType.Fire, PokemonType.Electric, PokemonType.Poison, PokemonType.Rock, PokemonType.Steel],
            notVeryEffective: [PokemonType.Grass, PokemonType.Bug],
            noEffect: [PokemonType.Flying]);

        // Flying
        SetTypeChartValues(PokemonType.Flying,
            superEffective: [PokemonType.Grass, PokemonType.Fighting, PokemonType.Bug],
            notVeryEffective: [PokemonType.Electric, PokemonType.Rock, PokemonType.Steel],
            noEffect: []);

        // Psychic
        SetTypeChartValues(PokemonType.Psychic,
            superEffective: [PokemonType.Fighting, PokemonType.Poison],
            notVeryEffective: [PokemonType.Psychic, PokemonType.Steel],
            noEffect: [PokemonType.Dark]);

        // Bug
        SetTypeChartValues(PokemonType.Bug,
            superEffective: [PokemonType.Grass, PokemonType.Psychic, PokemonType.Dark],
            notVeryEffective: [PokemonType.Fire, PokemonType.Fighting, PokemonType.Poison, PokemonType.Flying, PokemonType.Ghost, PokemonType.Steel, PokemonType.Fairy],
            noEffect: []);

        // Rock
        SetTypeChartValues(PokemonType.Rock,
            superEffective: [PokemonType.Fire, PokemonType.Ice, PokemonType.Flying, PokemonType.Bug],
            notVeryEffective: [PokemonType.Fighting, PokemonType.Ground, PokemonType.Steel],
            noEffect: []);

        // Ghost
        SetTypeChartValues(PokemonType.Ghost,
            superEffective: [PokemonType.Psychic, PokemonType.Ghost],
            notVeryEffective: [PokemonType.Dark],
            noEffect: [PokemonType.Normal]);

        // Dragon
        SetTypeChartValues(PokemonType.Dragon,
            superEffective: [PokemonType.Dragon],
            notVeryEffective: [PokemonType.Steel],
            noEffect: [PokemonType.Fairy]);

        // Dark
        SetTypeChartValues(PokemonType.Dark,
            superEffective: [PokemonType.Psychic, PokemonType.Ghost],
            notVeryEffective: [PokemonType.Fighting, PokemonType.Dark, PokemonType.Fairy],
            noEffect: []);

        // Steel
        SetTypeChartValues(PokemonType.Steel,
            superEffective: [PokemonType.Ice, PokemonType.Rock, PokemonType.Fairy],
            notVeryEffective: [PokemonType.Fire, PokemonType.Water, PokemonType.Electric, PokemonType.Steel],
            noEffect: []);

        // Fairy
        SetTypeChartValues(PokemonType.Fairy,
            superEffective: [PokemonType.Fighting, PokemonType.Dragon, PokemonType.Dark],
            notVeryEffective: [PokemonType.Fire, PokemonType.Poison, PokemonType.Steel],
            noEffect: []);

    }

    private static void SetTypeChartValues(PokemonType attackerType, PokemonType[] superEffective, PokemonType[] notVeryEffective, PokemonType[] noEffect)
    {
        foreach (var type in superEffective)
        {
            _typeChart[(int)attackerType, (int)type] = _superEffective;
        }
        foreach (var type in notVeryEffective)
        {
            _typeChart[(int)attackerType, (int)type] = _notVeryEffective;
        }
        foreach (var type in noEffect)
        {
            _typeChart[(int)attackerType, (int)type] = _noEffect;
        }
    }

    public static double GetMoveEffectiveness(Move attackerMove, Pokemon defender)
    {
        if (attackerMove == null) throw new ArgumentNullException(nameof(attackerMove), "Attacker move cannot be null.");
        if (defender == null) throw new ArgumentNullException(nameof(defender), "Defender cannot be null.");

        double effectiveness = _typeChart[(int)attackerMove.MoveType, (int)defender.FirstType];
        if (defender.SecondType.HasValue)
        {
            effectiveness *= _typeChart[(int)attackerMove.MoveType, (int)defender.SecondType.Value];
        }
        return effectiveness;
    }

}
