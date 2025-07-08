namespace PokemonBattleSimulator.Tests;

using PokemonBattleSimulator;

public class BattlePokemon_Tests
{
    private static readonly Pokemon pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric);
    private static readonly Pokemon bulbasaur = new Pokemon("Bulbasaur", 10, 20, 30, 40, 50, 60, 70, new Move("Vine Whip", 90, 100, 15, PokemonType.Grass, MoveCategory.Physical), PokemonType.Grass);
    private static readonly Pokemon multipleMovesPikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move[]
    {
        new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special),
        null!,
        new Move("Quick Attack", 40, 0, 30, PokemonType.Normal, MoveCategory.Physical),
        null!
    }, PokemonType.Electric);
    private static readonly Pokemon zeroAccuracyPikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move("Thunderbolt", 90, 0, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric);


    [Fact]
    public void CreateBattlePokemon_Success()
    {
        var battlePokemon = new BattlePokemon(pikachu, AIStrategies.AlwaysFirstValidMove);

        Assert.NotNull(battlePokemon);
        Assert.Equal(pikachu, battlePokemon.Pokemon);
        Assert.Equal(pikachu.Health, battlePokemon.CurrentHealth);
        Assert.False(battlePokemon.Fainted);
        Assert.Equal(pikachu.Moves.Length, battlePokemon.BattleMoves.Length);
        for (int i = 0; i < pikachu.Moves.Length; i++)
        {
            Assert.Equal(pikachu.Moves[i], battlePokemon.BattleMoves[i].Move);
        }
    }

    [Fact]
    public void TakeDamage_ReducesHealth()
    {
        var battlePokemon = new BattlePokemon(pikachu, AIStrategies.AlwaysFirstValidMove);
        int damage = 10;
        
        battlePokemon.TakeDamage(damage);
        Assert.Equal(pikachu.Health - damage, battlePokemon.CurrentHealth);
        Assert.False(battlePokemon.Fainted);
    }

    [Fact]
    public void TakeFatalDamage_FaintsPokemon()
    {
        var battlePokemon = new BattlePokemon(pikachu, AIStrategies.AlwaysFirstValidMove);
        int damage = pikachu.Health + 10; // More than current health
        
        battlePokemon.TakeDamage(damage);
        Assert.Equal(0, battlePokemon.CurrentHealth);
        Assert.True(battlePokemon.Fainted);
    }

    [Fact]
    public void TakeNegativeDamage_ThrowsException()
    {
        var battlePokemon = new BattlePokemon(pikachu, AIStrategies.AlwaysFirstValidMove);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => battlePokemon.TakeDamage(-10)); // Negative damage should throw exception
    }

    [Fact]
    public void GetMoveAlwaysFirstValidMove_ReturnsFirstValidMove()
    {
        var battlePokemon = new BattlePokemon(multipleMovesPikachu, AIStrategies.AlwaysFirstValidMove);
        var opponentPokemon = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);

        var move = battlePokemon.GetNextMove(opponentPokemon);
        Assert.Equal(multipleMovesPikachu.Moves[0], move.Move); // Should return the first valid move
    }

    [Fact]
    public void GetMoveRandomAvailable_ReturnsRandomValidMove()
    {
        var battlePokemon = new BattlePokemon(multipleMovesPikachu, AIStrategies.RandomMove);
        var opponentPokemon = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);
        var move = battlePokemon.GetNextMove(opponentPokemon);
        Assert.Contains(move.Move, multipleMovesPikachu.Moves); // Should return one of the available moves
    }

    [Fact]
    public void GetMoveBestOverall_ReturnsBestMoveAgainstOpponent()
    {
        var battlePokemon = new BattlePokemon(multipleMovesPikachu, AIStrategies.BestOverallMove);
        var opponentPokemon = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);
        var move = battlePokemon.GetNextMove(opponentPokemon);
        Assert.Equal(multipleMovesPikachu.Moves[0], move.Move); // Should return the best overall move against Bulbasaur
    }

    [Fact]
    public void GetMoveNoPP_FallbackMoveUsed()
    {
        var battlePokemon = new BattlePokemon(pikachu, AIStrategies.AlwaysFirstValidMove);
        var opponentPokemon = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);
        
        while (battlePokemon.BattleMoves[0].CurrentPP > 0)
        {
            battlePokemon.BattleMoves[0].UseMove(); // Use the first move until it has no PP left
        }

        var move = battlePokemon.GetNextMove(opponentPokemon);
        Assert.Equal(BattlePokemon.FallbackMove.Move, move.Move); // Should return the fallback move
    }

    [Fact]
    public void SimulateOneTurn_UpdatesPokemonState()
    {
        var battlePokemon = new BattlePokemon(pikachu, AIStrategies.AlwaysFirstValidMove);
        var opponentPokemon = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);
        // Simulate one turn
        var result = Battle.SimulateOneTurn(battlePokemon, opponentPokemon);
        // Check if the Pokemon's state is updated correctly
        Assert.NotEqual(pikachu.Health, battlePokemon.CurrentHealth); // Health should change after the turn
        Assert.False(battlePokemon.Fainted); // Should not faint
        Assert.Equal(TurnResult.BattleOngoing, result);
    }

    [Fact]
    public void SimulateOneTurn_FaintedPokemon_ReturnsFaintedResult()
    {
        var battlePokemon = new BattlePokemon(pikachu, AIStrategies.AlwaysFirstValidMove);
        var opponentPokemon = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);
        
        // Make Pikachu faint
        battlePokemon.TakeDamage(pikachu.Health + 10); // More than current health
        
        var result = Battle.SimulateOneTurn(battlePokemon, opponentPokemon);
        Assert.Equal(TurnResult.PokemonFainted, result); // Should return PokemonFainted result
    }

    [Fact]
    public void SimulateBattle_OnePokemonFaints_ReturnsBattleResult()
    {
        var battlePokemon1 = new BattlePokemon(pikachu, AIStrategies.AlwaysFirstValidMove);
        var battlePokemon2 = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);
        
        // Simulate the battle until one Pokemon faints
        var result = Battle.SimulateBattle(battlePokemon1, battlePokemon2);
        
        if (result == BattleResult.SecondPlayerWin)
        {
            Assert.True(battlePokemon1.Fainted);
            Assert.False(battlePokemon2.Fainted);
        }
        else if (result == BattleResult.FirstPlayerWin)
        {
            Assert.False(battlePokemon1.Fainted);
            Assert.True(battlePokemon2.Fainted);
        }
        else
        {
            Assert.Fail("Battle did not end as expected.");
        }
    }

    [Fact]
    public void SimulateOneTurn_ZeroAccuracyMove_OpponentTakesNoDamage()
    {
        var battlePokemon = new BattlePokemon(zeroAccuracyPikachu, AIStrategies.AlwaysFirstValidMove);
        var opponentPokemon = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);
        
        // Simulate one turn
        var result = Battle.SimulateOneTurn(battlePokemon, opponentPokemon);
        
        // Check if the opponent took no damage
        Assert.Equal(bulbasaur.Health, opponentPokemon.CurrentHealth); // Bulbasaur's health should remain unchanged
        Assert.Equal(TurnResult.BattleOngoing, result); // Should still be ongoing
    }
}