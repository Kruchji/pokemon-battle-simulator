using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Delegate representing a strategy for selecting a BattlePokemon to send out from own team based on the opponent's BattlePokemon.
/// </summary>
/// <param name="ownPokemonTeam">BattlePokemonTeam representing own team of Pokemon.</param>
/// <param name="opponentPokemon">BattlePokemon representing the opponent's Pokemon.</param>
/// <returns>BattlePokemon selected from own team.</returns>
internal delegate BattlePokemon AITeamStrategy(BattlePokemonTeam ownPokemonTeam, BattlePokemon opponentPokemon);

/// <summary>
/// Provides various strategies for selecting a BattlePokemon from own team in a Pokemon battle.
/// </summary>
internal static class AITeamStrategies
{
    /// <summary>
    /// Always returns the first valid BattlePokemon from own team that is not fainted.
    /// </summary>
    /// <param name="ownPokemonTeam">BattlePokemonTeam representing own team of Pokemon.</param>
    /// <param name="opponentPokemon">BattlePokemon representing the opponent's Pokemon.</param>
    /// <returns>BattlePokemon that is the first valid Pokemon in own team.</returns>
    /// <exception cref="ArgumentNullException">Pokemon team or opponent Pokemon is null.</exception>
    /// <exception cref="InvalidOperationException">No valid Pokemon available in the team.</exception>
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

    /// <summary>
    /// Selects a random available BattlePokemon from own team that is not fainted.
    /// </summary>
    /// <param name="ownPokemonTeam">BattlePokemonTeam representing own team of Pokemon.</param>
    /// <param name="opponentPokemon">BattlePokemon representing the opponent's Pokemon.</param>
    /// <returns>BattlePokemon that is randomly selected from own team.</returns>
    /// <exception cref="ArgumentNullException">Pokemon team or opponent Pokemon is null.</exception>
    /// <exception cref="InvalidOperationException">No valid Pokemon available in the team.</exception>
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

    /// <summary>
    /// Selects the BattlePokemon from own team that has the best overall move against the opponent's Pokemon.
    /// </summary>
    /// <param name="ownPokemonTeam">BattlePokemonTeam representing own team of Pokemon.</param>
    /// <param name="opponentPokemon">BattlePokemon representing the opponent's Pokemon.</param>
    /// <returns>BattlePokemon that has the best overall move against the opponent.</returns>
    /// <exception cref="ArgumentNullException">Pokemon team or opponent Pokemon is null.</exception>
    /// <exception cref="InvalidOperationException">No valid Pokemon available in the team.</exception>
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