namespace PokemonBattleSimulator.Tests;

using PokemonBattleSimulator;

public class PokemonTeam_Tests
{
    private static readonly Pokemon pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric);
    private static readonly Pokemon bulbasaur = new Pokemon("Bulbasaur", 10, 20, 30, 40, 50, 60, 70, new Move("Vine Whip", 90, 100, 15, PokemonType.Grass, MoveCategory.Physical), PokemonType.Grass);

    [Fact]
    public void CreatePokemonTeam_Success()
    {
        var team = new PokemonTeam("Test Team",pikachu);

        Assert.NotNull(team);
        Assert.Equal("Test Team", team.Name);
        Assert.Contains(pikachu, team.PokemonList);
    }

    [Fact]
    public void NullFirstPokemon_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new PokemonTeam("Invalid Team", (Pokemon)null!));
    }

    [Fact]
    public void AddPokemonToTeam_Success()
    {
        var team = new PokemonTeam("Test Team", pikachu);
        team.AddPokemon(1, bulbasaur);
        Assert.Contains(bulbasaur, team.PokemonList);
        Assert.Equal(bulbasaur, team.PokemonList[1]);
    }

    [Fact]
    public void AddPokemonOutOfRange_ThrowsException()
    {
        var team = new PokemonTeam("Test Team", pikachu);
        Assert.Throws<ArgumentOutOfRangeException>(() => team.AddPokemon(PokemonTeam.MaxTeamSize, bulbasaur)); // Index 6 is out of range
    }

    [Fact]
    public void AddNullPokemonToTeamAtIndex0_ThrowsException()
    {
        var team = new PokemonTeam("Test Team", pikachu);
        Assert.Throws<ArgumentNullException>(() => team.AddPokemon(0, null!)); // Cannot add null at index 0
    }

    [Fact]
    public void AddNullPokemonToTeamAtOtherIndex_Success()
    {
        var team = new PokemonTeam("Test Team", pikachu);
        team.AddPokemon(1, bulbasaur); // Add a valid Pokemon first
        team.AddPokemon(1, null!); // Adding null at index 1 is allowed
        Assert.Null(team.PokemonList[1]); // Index 1 should be null
    }

}