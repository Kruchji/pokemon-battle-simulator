using System;

namespace PokemonBattleSimulator;

// Returns Move to use
public delegate Move AIStrategy(BattlePokemon ownPokemon, BattlePokemon opponentPokemon);


// TODO: Conside that move on any position may be null, so check for it
public static class AIStrategies
{
    // Strategy that always returns the first move
    public static Move AlwaysFirstMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");
        // Return the first move of the own Pokemon
        return ownPokemon.Pokemon.Moves[0];
    }

    // Strategy returning a random move
    public static Move RandomMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");

        // Generate a random index for the (not null) move
        Random random = new Random();
        int randomIndex = random.Next(0, ownPokemon.Pokemon.SetMoveIndices.Count);

        // Return the randomly selected move
        return ownPokemon.Pokemon.Moves[ownPokemon.Pokemon.SetMoveIndices[randomIndex]];
    }

    // Strategy returning a move based on type effectiveness (most effective move)
    public static Move MostEffectiveMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");

        Move bestMove = ownPokemon.Pokemon.Moves[0];
        double bestEffectiveness = double.MinValue;

        foreach (var move in ownPokemon.Pokemon.Moves)
        {
            if (move == null) continue; // Skip null moves
            double effectiveness = TypeCalculator.GetMoveEffectiveness(move, opponentPokemon.Pokemon);
            if (effectiveness > bestEffectiveness)
            {
                bestEffectiveness = effectiveness;
                bestMove = move;
            }
        }

        return bestMove;
    }

    // Strategy returning most powerful move (highest power)
    public static Move MostPowerfulMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");

        Move bestMove = ownPokemon.Pokemon.Moves[0];
        int highestPower = 0;

        foreach (var move in ownPokemon.Pokemon.Moves)
        {
            if (move == null) continue; // Skip null moves
            if (move.Power > highestPower)
            {
                highestPower = move.Power;
                bestMove = move;
            }
        }

        return bestMove;
    }

    // Strategy returning most accurate move
    public static Move MostAccurateMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");
        Move bestMove = ownPokemon.Pokemon.Moves[0];
        int highestAccuracy = 0;
        foreach (var move in ownPokemon.Pokemon.Moves)
        {
            if (move == null) continue; // Skip null moves
            if (move.Accuracy > highestAccuracy)
            {
                highestAccuracy = move.Accuracy;
                bestMove = move;
            }
        }
        return bestMove;
    }

    // Strategy considering move effectiveness, power, and accuracy (multiplied)
    public static Move BestOverallMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");
        Move bestMove = ownPokemon.Pokemon.Moves[0];
        double bestScore = double.MinValue;
        foreach (var move in ownPokemon.Pokemon.Moves)
        {
            if (move == null) continue; // Skip null moves
            double effectiveness = TypeCalculator.GetMoveEffectiveness(move, opponentPokemon.Pokemon);
            double score = effectiveness * move.Power * (move.Accuracy / 100.0); // Normalize accuracy to a 0-1 scale
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }
        return bestMove;
    }
}


internal delegate int AITeamStrategy(BattlePokemonTeam ownPokemonTeam, BattlePokemonTeam opponentPokemonTeam);
