using System;

namespace PokemonBattleSimulator;

// Returns BattlePokemon to send out
internal delegate BattlePokemon AITeamStrategy(BattlePokemonTeam ownPokemonTeam, BattlePokemon opponentPokemon);

internal static class AITeamStrategies
{
    // Strategy that always picks the next valid BattlePokemon
    public static BattlePokemon AlwaysFirstValidPokemon(BattlePokemonTeam ownPokemonTeam, BattlePokemon opponentPokemon)
    {
        if (ownPokemonTeam == null) throw new ArgumentNullException(nameof(ownPokemonTeam), "Own Pokemon team cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");

        // Return the first (not null and alive) BattlePokemon from own team
        foreach (var battlePokemon in ownPokemonTeam.BattlePokemonList)
        {
            if (battlePokemon != null && !battlePokemon.Fainted)
            {
                return battlePokemon;
            }
        }

        throw new InvalidOperationException("No valid Pokemon available in the team.");
    }

    // Random available BattlePokemon (not fainted)
    public static BattlePokemon RandomAvailablePokemon(BattlePokemonTeam ownPokemonTeam, BattlePokemon opponentPokemon)
    {
        if (ownPokemonTeam == null) throw new ArgumentNullException(nameof(ownPokemonTeam), "Own Pokemon team cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");

        Random random = new Random();
        var availablePokemons = ownPokemonTeam.BattlePokemonList.Where(battlePokemon => battlePokemon != null && !battlePokemon.Fainted).ToList();
        if (availablePokemons.Count == 0)
        {
            throw new InvalidOperationException("No available Pokemon in the team.");
        }
        int randomIndex = random.Next(availablePokemons.Count);
        return availablePokemons[randomIndex];
    }

    // Get Pokemon that uses the most effective move against the opponent
    public static BattlePokemon BestOverallMovePokemon(BattlePokemonTeam ownPokemonTeam, BattlePokemon opponentPokemon)
    {
        if (ownPokemonTeam == null) throw new ArgumentNullException(nameof(ownPokemonTeam), "Own Pokemon team cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");

        BattlePokemon bestPokemon = null!;
        double bestScore = double.MinValue;
        foreach (var battlePokemon in ownPokemonTeam.BattlePokemonList)
        {
            if (battlePokemon == null || battlePokemon.Fainted) continue; // Skip null or fainted Pokemon

            // Get the move that the Pokemon would use against the opponent (if selected)
            var battleMove = battlePokemon.GetNextMove(opponentPokemon);

            // Get the move effectiveness
            double effectiveness = TypeCalculator.GetMoveEffectiveness(battleMove.Move, opponentPokemon.Pokemon);
            double score = effectiveness * battleMove.Move.Power * (battleMove.Move.Accuracy / 100.0); // Normalize accuracy to a 0-1 scale
            if (score > bestScore)
            {
                bestScore = score;
                bestPokemon = battlePokemon;
            }
        }

        if (bestPokemon == null)
        {
            throw new InvalidOperationException("No valid Pokemon available in the team.");
        }
        return bestPokemon;
    }
}