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
    private static bool SimulateOneTurn(BattlePokemon firstPokemon, BattlePokemon secondPokemon, IPrefixedConsole? prefConsole = null)
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
        BattleMove fasterBattleMove = fasterPokemon.GetNextMove(slowerPokemon);

        fasterBattleMove.UseMove(); // Decrease PP
        prefConsole?.WriteLine($"{fasterPokemon.Name} used {fasterBattleMove.Move.Name}.");

        // Check if slower move hit (accuracy)
        if (_randomGenerator.NextDouble() < (double)fasterBattleMove.Move.Accuracy / 100)
        {
            // Calculate and apply damage to slower
            int damageToSlower = CalculateDamage(fasterPokemon.Pokemon, slowerPokemon.Pokemon, fasterBattleMove.Move);
            slowerPokemon.TakeDamage(damageToSlower);

            prefConsole?.WriteLine($"{slowerPokemon.Name} took {damageToSlower} damage.");

            if (slowerPokemon.Fainted) return true; // If slower fainted, no need to continue
        }
        else
        {
            prefConsole?.WriteLine($"{fasterPokemon.Name} missed.");
        }

        // Get slow move
        BattleMove slowerBattleMove = slowerPokemon.GetNextMove(fasterPokemon);

        slowerBattleMove.UseMove(); // Decrease PP
        prefConsole?.WriteLine($"{slowerPokemon.Name} used {slowerBattleMove.Move.Name}.");

        // Check if faster move hit (accuracy)
        if (_randomGenerator.NextDouble() < (double)slowerBattleMove.Move.Accuracy / 100)
        {
            // Calculate and apply damage to faster
            int damageToFaster = CalculateDamage(slowerPokemon.Pokemon, fasterPokemon.Pokemon, slowerBattleMove.Move);
            fasterPokemon.TakeDamage(damageToFaster);

            prefConsole?.WriteLine($"{fasterPokemon.Name} took {damageToFaster} damage.");

            if (fasterPokemon.Fainted) return true; // If faster fainted, battle ends
        }
        else
        {
            prefConsole?.WriteLine($"{slowerPokemon.Name} missed.");
        }

        return false; // Both Pokemon are still standing
    }

    // true => firstPokemon won, false => secondPokemon won
    // TODO: replace with enum for battle result?
    public static bool SimulateBattle(BattlePokemon firstPokemon, BattlePokemon secondPokemon, IPrefixedConsole? prefConsole = null)
    {
        if (firstPokemon == null) throw new ArgumentNullException(nameof(firstPokemon), "First Pokemon cannot be null.");
        if (secondPokemon == null) throw new ArgumentNullException(nameof(secondPokemon), "Second Pokemon cannot be null.");

        bool battleEnded = false;
        int turnNumber = 0;
        while (!battleEnded)
        {
            // Simulate one turn of battle
            turnNumber++;

            prefConsole?.WriteLine($"");
            prefConsole?.WriteLine($"Turn {turnNumber}:");
            prefConsole?.WriteLine($"Status: {firstPokemon.Name} - {firstPokemon.CurrentHealth}/{firstPokemon.Health} HP, {secondPokemon.Name} - {secondPokemon.CurrentHealth}/{secondPokemon.Health} HP");

            battleEnded = SimulateOneTurn(firstPokemon, secondPokemon, prefConsole);
        }

        // Return true if firstPokemon won, false if secondPokemon won
        if (firstPokemon.Fainted && !secondPokemon.Fainted)
        {
            prefConsole?.WriteLine($"");
            prefConsole?.WriteLine($"{firstPokemon.Name} fainted!");
            prefConsole?.WriteLine($"{secondPokemon.Name} won the battle!");
            return false; // Second Pokemon won
        }
        else if (secondPokemon.Fainted && !firstPokemon.Fainted)
        {
            prefConsole?.WriteLine($"");
            prefConsole?.WriteLine($"{secondPokemon.Name} fainted!");
            prefConsole?.WriteLine($"{firstPokemon.Name} won the battle!");
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

    public static (int, int) SimulateManyTeamBattles(BattlePokemonTeam firstTeam, BattlePokemonTeam secondTeam, int battleCount)
    {
        if (firstTeam == null) throw new ArgumentNullException(nameof(firstTeam), "First team cannot be null.");
        if (secondTeam == null) throw new ArgumentNullException(nameof(secondTeam), "Second team cannot be null.");
        if (battleCount <= 0) throw new ArgumentOutOfRangeException(nameof(battleCount), "Battle count must be greater than zero.");

        // Track wins for each team
        int firstWins = 0;
        int secondWins = 0;

        // Run all battles in parallel
        Parallel.For(0, battleCount, _ =>
        {
            // Clone the teams for each battle to reset their state
            BattlePokemonTeam firstTeamClone = new BattlePokemonTeam(firstTeam);
            BattlePokemonTeam secondTeamClone = new BattlePokemonTeam(secondTeam);

            bool firstWon = SimulateTeamBattle(firstTeamClone, secondTeamClone, null); // No console output
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

    public static bool SimulateTeamBattle(BattlePokemonTeam firstTeam, BattlePokemonTeam secondTeam, IPrefixedConsole? prefConsole = null)
    {
        if (firstTeam == null) throw new ArgumentNullException(nameof(firstTeam), "First team cannot be null.");
        if (secondTeam == null) throw new ArgumentNullException(nameof(secondTeam), "Second team cannot be null.");

        // First pick the leading (first) Pokemon for each team and start battle between them
        BattlePokemon firstLeadingPokemon = firstTeam.BattlePokemonList[0] ?? throw new InvalidOperationException("First team has no valid Pokemon.");
        BattlePokemon secondLeadingPokemon = secondTeam.BattlePokemonList[0] ?? throw new InvalidOperationException("Second team has no valid Pokemon.");

        prefConsole?.WriteLine($"Team {firstTeam.Name} sent out {firstLeadingPokemon.Name}!");
        prefConsole?.WriteLine($"Team {secondTeam.Name} sent out {secondLeadingPokemon.Name}!");

        // Simulate battle between the leading Pokemon
        bool firstWon = SimulateBattle(firstLeadingPokemon, secondLeadingPokemon, prefConsole);

        // Mark the surviving Pokemon as active
        BattlePokemon firstTeamActivePokemon = null!;
        BattlePokemon secondTeamActivePokemon = null!;
        if (firstWon)
        {
            firstTeamActivePokemon = firstLeadingPokemon;
        }
        else
        {
            secondTeamActivePokemon = secondLeadingPokemon;
        }

        // Continue the battle until one team has all Pokemon fainted
        while (!firstTeam.AllPokemonFainted && !secondTeam.AllPokemonFainted)
        {
            // Pick next Pokemon for the team that lost the previous battle
            if (firstWon)
            {
                // First team won the previous battle, so pick the next Pokemon for the second team
                secondTeamActivePokemon = secondTeam.PickNextBattlePokemon(firstTeamActivePokemon);
                prefConsole?.WriteLine($"Team {secondTeam.Name} sent out {secondTeamActivePokemon.Name}!");
            }
            else
            {
                // Second team won the previous battle, so pick the next Pokemon for the first team
                firstTeamActivePokemon = firstTeam.PickNextBattlePokemon(secondTeamActivePokemon);
                prefConsole?.WriteLine($"Team {firstTeam.Name} sent out {firstTeamActivePokemon.Name}!");
            }

            // Simulate battle between the active Pokemon
            firstWon = SimulateBattle(firstTeamActivePokemon, secondTeamActivePokemon, prefConsole);
        }

        // Determine the winner
        if (firstTeam.AllPokemonFainted && !secondTeam.AllPokemonFainted)
        {
            prefConsole?.WriteLine($"Team {firstTeam.Name} has no Pokemon left! Team {secondTeam.Name} wins!");
            return false; // Second team won
        }
        else if (secondTeam.AllPokemonFainted && !firstTeam.AllPokemonFainted)
        {
            prefConsole?.WriteLine($"Team {secondTeam.Name} has no Pokemon left! Team {firstTeam.Name} team wins!");
            return true; // First team won
        }
        else
        {
            throw new InvalidOperationException("Both teams have all Pokemon fainted at the same time. This should not happen in a basic team battle.");
        }
    }
}
