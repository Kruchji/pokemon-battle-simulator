using System;

namespace PokemonBattleSimulator;

public static class DamageCalculator
{
    private static readonly double _criticalHitChance = 1 / 24; // about 4.17% (Gen VII onwards)
    private static readonly double _criticalHitMultiplier = 1.5; // Critical hit damage multiplier (Gen VI onwards)
    private static readonly double _stabMultiplier = 1.5; // Same Type Attack Bonus (STAB) multiplier

    private static readonly Random _randomGenerator = new Random();

    public static int CalculateDamage(Pokemon attacker, Pokemon defender, Move move)
    {
        if (attacker == null) throw new ArgumentNullException(nameof(attacker), "Attacker cannot be null.");
        if (defender == null) throw new ArgumentNullException(nameof(defender), "Defender cannot be null.");
        if (move == null) throw new ArgumentNullException(nameof(move), "Move cannot be null.");

        // Base damage calculation formula
        double damage = ((2 * attacker.Level / 5 + 2) * move.Power * attacker.Attack / defender.Defense) / 50 + 2;

        // Critical hit check
        bool isCriticalHit = _randomGenerator.NextDouble() < _criticalHitChance;
        if (isCriticalHit)
        {
            damage *= _criticalHitMultiplier;
        }

        // Random factor
        double randomFactor = 0.85 + _randomGenerator.NextDouble() * 0.15; // Random factor between 0.85 and 1.00
        damage *= randomFactor;

        // STAB (Same Type Attack Bonus) check
        if (attacker.FirstType == move.MoveType || (attacker.SecondType.HasValue && attacker.SecondType.Value == move.MoveType))
        {
            damage *= _stabMultiplier;
        }

        // Type effectiveness calculation
        double typeEffectiveness = TypeCalculator.GetMoveEffectiveness(move, defender);
        damage *= typeEffectiveness;


        return (int)Math.Max(damage, 1); // Ensure damage is at least 1
    }
}
