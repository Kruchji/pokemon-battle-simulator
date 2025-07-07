using System;

namespace PokemonBattleSimulator;

// Returns Move to use
internal delegate BattleMove AIStrategy(BattlePokemon ownPokemon, BattlePokemon opponentPokemon);


internal static class AIStrategies
{
    // Strategy that always returns the first move
    public static BattleMove AlwaysFirstValidMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");
        // Return the first (not null, with pp) move of own Pokemon
        foreach (var battleMove in ownPokemon.BattleMoves)
        {
            if (battleMove.Move != null && battleMove.CurrentPP > 0) // Ensure the move is not null and has PP left
            {
                return battleMove;
            }
        }
        throw new InvalidOperationException("No valid moves available for this Pokemon."); // Strategies are not invoked when all moves are invalid
    }

    // Strategy returning a random move
    public static BattleMove RandomMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");

        // Generate a random index for the (not null) move
        Random random = new Random();
        var availableMoves = ownPokemon.BattleMoves.Where(battleMove => battleMove.Move != null && battleMove.CurrentPP > 0).ToList();

        if (availableMoves.Count == 0)
        {
            throw new InvalidOperationException("No valid moves available for this Pokemon."); // Strategies are not invoked when all moves are invalid
        }

        int randomIndex = random.Next(availableMoves.Count);
        return availableMoves[randomIndex]; // Return a random valid move
    }

    // Strategy returning a move based on type effectiveness (most effective move)
    public static BattleMove MostEffectiveMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");

        BattleMove bestMove = null!;
        double bestEffectiveness = double.MinValue;

        foreach (var battleMove in ownPokemon.BattleMoves)
        {
            if (battleMove.Move == null) continue; // Skip null moves
            if (battleMove.CurrentPP == 0) continue; // Skip moves with 0 PP
            double effectiveness = TypeCalculator.GetMoveEffectiveness(battleMove.Move, opponentPokemon.Pokemon);
            if (effectiveness > bestEffectiveness)
            {
                bestEffectiveness = effectiveness;
                bestMove = battleMove;
            }
        }

        // If no move picked
        if (bestMove == null)
        {
            throw new InvalidOperationException("No valid moves available for this Pokemon.");  // Strategies are not invoked when all moves are invalid
        }

        return bestMove; // Return the most effective move found
    }

    // Strategy returning most powerful move (highest power)
    public static BattleMove MostPowerfulMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");

        BattleMove bestMove = null!;
        int highestPower = 0;

        foreach (var battleMove in ownPokemon.BattleMoves)
        {
            if (battleMove.Move == null) continue; // Skip null moves
            if (battleMove.CurrentPP == 0) continue; // Skip moves with 0 PP
            if (battleMove.Move.Power > highestPower)
            {
                highestPower = battleMove.Move.Power;
                bestMove = battleMove;
            }
        }

        // If no move picked
        if (bestMove == null)
        {
            throw new InvalidOperationException("No valid moves available for this Pokemon.");
        }

        return bestMove;
    }

    // Strategy returning most accurate move
    public static BattleMove MostAccurateMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");
        BattleMove bestMove = null!;
        int highestAccuracy = 0;
        foreach (var battleMove in ownPokemon.BattleMoves)
        {
            if (battleMove.Move == null) continue; // Skip null moves
            if (battleMove.CurrentPP == 0) continue; // Skip moves with 0 PP
            if (battleMove.Move.Accuracy > highestAccuracy)
            {
                highestAccuracy = battleMove.Move.Accuracy;
                bestMove = battleMove;
            }
        }

        // If no move picked
        if (bestMove == null)
        {
            throw new InvalidOperationException("No valid moves available for this Pokemon.");
        }

        return bestMove;
    }

    // Strategy considering move effectiveness, power, and accuracy (multiplied)
    public static BattleMove BestOverallMove(BattlePokemon ownPokemon, BattlePokemon opponentPokemon)
    {
        if (ownPokemon == null) throw new ArgumentNullException(nameof(ownPokemon), "Own Pokemon cannot be null.");
        if (opponentPokemon == null) throw new ArgumentNullException(nameof(opponentPokemon), "Opponent Pokemon cannot be null.");
        BattleMove bestMove = null!;
        double bestScore = double.MinValue;
        foreach (var battleMove in ownPokemon.BattleMoves)
        {
            if (battleMove.Move == null) continue; // Skip null moves
            if (battleMove.CurrentPP == 0) continue; // Skip moves with 0 PP
            double effectiveness = TypeCalculator.GetMoveEffectiveness(battleMove.Move, opponentPokemon.Pokemon);
            double score = effectiveness * battleMove.Move.Power * (battleMove.Move.Accuracy / 100.0); // Normalize accuracy to a 0-1 scale
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = battleMove;
            }
        }

        // If no move picked
        if (bestMove == null)
        {
            throw new InvalidOperationException("No valid moves available for this Pokemon.");
        }

        return bestMove;
    }
}
