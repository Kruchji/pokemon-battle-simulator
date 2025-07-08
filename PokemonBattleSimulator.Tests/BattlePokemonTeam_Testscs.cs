namespace PokemonBattleSimulator.Tests;

using PokemonBattleSimulator;

public class BattlePokemonTeam_Tests
{
    private static readonly Move thunderbolt = new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special);
    private static readonly Pokemon pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, thunderbolt, PokemonType.Electric);
    private static readonly Pokemon bulbasaur = new Pokemon("Bulbasaur", 10, 20, 30, 40, 50, 60, 70, new Move("Vine Whip", 90, 100, 15, PokemonType.Grass, MoveCategory.Physical), PokemonType.Grass);
    private static readonly Pokemon charmander = new Pokemon("Charmander", 10, 20, 30, 40, 50, 60, 70, new Move("Ember", 90, 100, 15, PokemonType.Fire, MoveCategory.Special), PokemonType.Fire);
    private static readonly PokemonTeam team = new PokemonTeam("Ash's Team", pikachu);
    private static readonly PokemonTeam multiplePokemonTeam = new PokemonTeam("Multiple Pokemon Team", new[] { pikachu, charmander, null!, null!, null!, null! });

    [Fact]
    public void CreateBattlePokemonTeam_Success()
    {
        var battleTeam = new BattlePokemonTeam(team, AIStrategies.AlwaysFirstValidMove, AITeamStrategies.AlwaysFirstValidPokemon);

        Assert.NotNull(battleTeam);
        Assert.Equal(team.Name, battleTeam.Name);
        Assert.Equal(pikachu, battleTeam.BattlePokemonList[0].Pokemon);
        Assert.False(battleTeam.AllPokemonFainted);
    }

    [Fact]
    public void PickNextPokemon_FirstPokemonAvailable_Success()
    {
        var battleTeam = new BattlePokemonTeam(team, AIStrategies.AlwaysFirstValidMove, AITeamStrategies.AlwaysFirstValidPokemon);
        var opponentPokemon = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);
        var nextPokemon = battleTeam.PickNextBattlePokemon(opponentPokemon);
        Assert.NotNull(nextPokemon);
        Assert.Equal(pikachu, nextPokemon.Pokemon);
    }

    [Fact]
    public void PickNextPokemon_BestOverall_Success()
    {
        var battleTeam = new BattlePokemonTeam(multiplePokemonTeam, AIStrategies.BestOverallMove, AITeamStrategies.BestOverallMovePokemon);
        var opponentPokemon = new BattlePokemon(bulbasaur, AIStrategies.AlwaysFirstValidMove);
        var nextPokemon = battleTeam.PickNextBattlePokemon(opponentPokemon);
        
        Assert.NotNull(nextPokemon);
        Assert.Equal(charmander, nextPokemon.Pokemon); // Charmander is the best overall choice against Bulbasaur
    }

    [Fact]
    public void SimulateBattle_LosingTeamFainted()
    {
        var firstTeam = new BattlePokemonTeam(team, AIStrategies.AlwaysFirstValidMove, AITeamStrategies.AlwaysFirstValidPokemon);
        var secondTeam = new BattlePokemonTeam(multiplePokemonTeam, AIStrategies.AlwaysFirstValidMove, AITeamStrategies.AlwaysFirstValidPokemon);

        var result = Battle.SimulateTeamBattle(firstTeam, secondTeam);

        if (result == BattleResult.SecondPlayerWin)
        {
            Assert.True(firstTeam.AllPokemonFainted);
            Assert.False(secondTeam.AllPokemonFainted);
        }
        else if (result == BattleResult.FirstPlayerWin)
        {
            Assert.False(firstTeam.AllPokemonFainted);
            Assert.True(secondTeam.AllPokemonFainted);
        }
        else
        {
            Assert.Fail("Unexpected battle result: " + result);
        }
    }
}