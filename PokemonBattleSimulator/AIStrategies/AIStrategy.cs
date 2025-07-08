using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Delegate representing an AI strategy for selecting a move in a battle (to use against opponent).
/// </summary>
/// <param name="ownPokemon">BattlePokemon representing the AI's own Pokemon.</param>
/// <param name="opponentPokemon">Opponent's BattlePokemon.</param>
/// <returns>BattleMove selected by the AI.</returns>
internal delegate BattleMove AIStrategy(BattlePokemon ownPokemon, BattlePokemon opponentPokemon);

/// <summary>
/// Provides various AI strategies for selecting moves in a Pokemon battle.
/// </summary>
internal static class AIStrategies
{
    /// <summary>
    /// Always returns the first valid move of own Pokemon that has PP left.
    /// </summary>
    /// <param name="ownPokemon">BattlePokemon representing the AI's own Pokemon.</param>
    /// <param name="opponentPokemon">Opponent's BattlePokemon.</param>
    /// <returns>BattleMove that is the first valid move of own Pokemon.</returns>
    /// <exception cref="ArgumentNullException">Any Pokemon is null.</exception>
    /// <exception cref="InvalidOperationException">No valid moves available for this Pokemon.</exception>
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

    /// <summary>
    /// Selects a random valid move from own Pokemon's moves that has PP left.
    /// </summary>
    /// <param name="ownPokemon">BattlePokemon representing the AI's own Pokemon.</param>
    /// <param name="opponentPokemon">Opponent's BattlePokemon.</param>
    /// <returns>BattleMove selected randomly from own Pokemon's valid moves.</returns>
    /// <exception cref="ArgumentNullException">Any Pokemon is null.</exception>
    /// <exception cref="InvalidOperationException">No valid moves available for this Pokemon.</exception>
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

    /// <summary>
    /// Selects the most effective move against the opponent's Pokemon based on type effectiveness.
    /// </summary>
    /// <param name="ownPokemon">BattlePokemon representing the AI's own Pokemon.</param>
    /// <param name="opponentPokemon">Opponent's BattlePokemon.</param>
    /// <returns>BattleMove that is the most effective against the opponent's Pokemon.</returns>
    /// <exception cref="ArgumentNullException">Any Pokemon is null.</exception>
    /// <exception cref="InvalidOperationException">No valid moves available for this Pokemon.</exception>
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

    /// <summary>
    /// Selects the most powerful move from own Pokemon's moves based on move power.
    /// </summary>
    /// <param name="ownPokemon">BattlePokemon representing the AI's own Pokemon.</param>
    /// <param name="opponentPokemon">Opponent's BattlePokemon.</param>
    /// <returns>BattleMove that has the highest power among own Pokemon's moves.</returns>
    /// <exception cref="ArgumentNullException">Any Pokemon is null.</exception>
    /// <exception cref="InvalidOperationException">No valid moves available for this Pokemon.</exception>
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

    /// <summary>
    /// Selects the most accurate move from own Pokemon's moves based on move accuracy.
    /// </summary>
    /// <param name="ownPokemon">BattlePokemon representing the AI's own Pokemon.</param>
    /// <param name="opponentPokemon">Opponent's BattlePokemon.</param>
    /// <returns>BattleMove that has the highest accuracy among own Pokemon's moves.</returns>
    /// <exception cref="ArgumentNullException">Any Pokemon is null.</exception>
    /// <exception cref="InvalidOperationException">No valid moves available for this Pokemon.</exception>
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

    /// <summary>
    /// Selects the best overall move based on effectiveness, power, and accuracy.
    /// </summary>
    /// <param name="ownPokemon">BattlePokemon representing the AI's own Pokemon.</param>
    /// <param name="opponentPokemon">Opponent's BattlePokemon.</param>
    /// <returns>BattleMove that is considered the best overall move.</returns>
    /// <exception cref="ArgumentNullException">Any Pokemon is null.</exception>
    /// <exception cref="InvalidOperationException">No valid moves available for this Pokemon.</exception>
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
