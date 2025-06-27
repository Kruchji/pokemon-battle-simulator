using System;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonBattleSimulator;

internal static class Battle
{
    private static readonly double _criticalHitChance = 1 / 24; // ≈4.17% (Gen VII onwards)
    private static readonly double _criticalHitMultiplier = 1.5; // Critical hit damage multiplier (Gen VI onwards)
    private static readonly double _stabMultiplier = 1.5; // Same Type Attack Bonus (STAB) multiplier

    private static readonly Random _randomGenerator = new Random();

    private static int CalculateDamage(Pokemon attacker, Pokemon defender, Move move)
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

    // Returns true if the battle ended (one Pokemon fainted), otherwise false
    private static bool SimulateOneTurn(BattlePokemon firstPokemon, BattlePokemon secondPokemon, IConsoleWriter? consoleWriter = null)
    {
        if (firstPokemon == null) throw new ArgumentNullException(nameof(firstPokemon), "First Pokemon cannot be null.");
        if (secondPokemon == null) throw new ArgumentNullException(nameof(secondPokemon), "Second Pokemon cannot be null.");

        // Determine the order of actions based on Speed (on tie, randomly choose)
        BattlePokemon fasterPokemon, slowerPokemon;
        if (firstPokemon.Speed > secondPokemon.Speed)
        {
            fasterPokemon = firstPokemon;
            slowerPokemon = secondPokemon;
        }
        else if (firstPokemon.Speed < secondPokemon.Speed)
        {
            fasterPokemon = secondPokemon;
            slowerPokemon = firstPokemon;
        }
        else // Speed tie
        {
            fasterPokemon = _randomGenerator.Next(2) == 0 ? firstPokemon : secondPokemon;
            slowerPokemon = fasterPokemon == firstPokemon ? secondPokemon : firstPokemon;
        }

        // Execute moves

        // Get fast move
        Move fasterMove = fasterPokemon.GetNextMove(slowerPokemon);

        consoleWriter?.WriteLine($"{fasterPokemon.Name} used {fasterMove.Name}.");

        // Check if slower move hit (accuracy)
        if (_randomGenerator.NextDouble() < (double)fasterMove.Accuracy / 100)
        {
            // Calculate and apply damage to slower
            int damageToSlower = CalculateDamage(fasterPokemon.Pokemon, slowerPokemon.Pokemon, fasterMove);
            slowerPokemon.TakeDamage(damageToSlower);

            consoleWriter?.WriteLine($"{slowerPokemon.Name} took {damageToSlower} damage.");

            if (slowerPokemon.Fainted) return true; // If slower fainted, no need to continue
        }
        else
        {
            consoleWriter?.WriteLine($"{fasterPokemon.Name} missed.");
        }

        // Get slow move
        Move slowerMove = slowerPokemon.GetNextMove(fasterPokemon);
        consoleWriter?.WriteLine($"{slowerPokemon.Name} used {slowerMove.Name}.");

        // Check if faster move hit (accuracy)
        if (_randomGenerator.NextDouble() < (double)slowerMove.Accuracy / 100)
        {
            // Calculate and apply damage to faster
            int damageToFaster = CalculateDamage(slowerPokemon.Pokemon, fasterPokemon.Pokemon, slowerMove);
            fasterPokemon.TakeDamage(damageToFaster);

            consoleWriter?.WriteLine($"{fasterPokemon.Name} took {damageToFaster} damage.");

            if (fasterPokemon.Fainted) return true; // If faster fainted, battle ends
        }
        else
        {
            consoleWriter?.WriteLine($"{slowerPokemon.Name} missed.");
        }

        return false; // Both Pokemon are still standing
    }

    // true => firstPokemon won, false => secondPokemon won
    // TODO: replace with enum for battle result?
    public static bool SimulateBattle(BattlePokemon firstPokemon, BattlePokemon secondPokemon, IConsoleWriter? consoleWriter = null)
    {
        if (firstPokemon == null) throw new ArgumentNullException(nameof(firstPokemon), "First Pokemon cannot be null.");
        if (secondPokemon == null) throw new ArgumentNullException(nameof(secondPokemon), "Second Pokemon cannot be null.");

        bool battleEnded = false;
        int turnNumber = 0;
        while (!battleEnded)
        {
            // Simulate one turn of battle
            turnNumber++;

            consoleWriter?.WriteLine($"\nTurn {turnNumber}:");
            consoleWriter?.WriteLine($"Status: {firstPokemon.Name} - {firstPokemon.CurrentHealth}/{firstPokemon.Health} HP, {secondPokemon.Name} - {secondPokemon.CurrentHealth}/{secondPokemon.Health} HP");

            battleEnded = SimulateOneTurn(firstPokemon, secondPokemon, consoleWriter);
        }

        // Return true if firstPokemon won, false if secondPokemon won
        if (firstPokemon.Fainted && !secondPokemon.Fainted)
        {
            consoleWriter?.WriteLine($"\n{firstPokemon.Name} fainted!");
            consoleWriter?.WriteLine($"{secondPokemon.Name} won the battle!");
            return false; // Second Pokemon won
        }
        else if (secondPokemon.Fainted && !firstPokemon.Fainted)
        {
            consoleWriter?.WriteLine($"\n{secondPokemon.Name} fainted!");
            consoleWriter?.WriteLine($"{firstPokemon.Name} won the battle!");
            return true; // First Pokemon won
        }
        else
        {
            throw new InvalidOperationException("Both Pokemon fainted at the same time. This should not happen in a basic battle.");
        }
    }

    public static (int, int) SimulateManyBattles(BattlePokemon firstPokemon, BattlePokemon secondPokemon, int battleCount)
    {
        if (firstPokemon == null) throw new ArgumentNullException(nameof(firstPokemon), "First Pokemon cannot be null.");
        if (secondPokemon == null) throw new ArgumentNullException(nameof(secondPokemon), "Second Pokemon cannot be null.");
        if (battleCount <= 0) throw new ArgumentOutOfRangeException(nameof(battleCount), "Battle count must be greater than zero.");
        int firstWins = 0;
        int secondWins = 0;

        // TODO: Make this group more battles into one task?

        // Run all battles in parallel
        Parallel.For(0, battleCount, _ =>
        {
            // Clone the Pokemon for each battle to reset their state
            BattlePokemon firstClone = new BattlePokemon(firstPokemon);
            BattlePokemon secondClone = new BattlePokemon(secondPokemon);
            bool firstWon = SimulateBattle(firstClone, secondClone, null); // No console output
            if (firstWon)
            {
                Interlocked.Increment(ref firstWins);
            }
            else
            {
                Interlocked.Increment(ref secondWins);
            }
        });

        return (firstWins, secondWins);
    }
}
