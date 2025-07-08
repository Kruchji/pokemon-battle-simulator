namespace PokemonBattleSimulator.Tests;

using PokemonBattleSimulator;

public class User_Tests
{
    private static readonly Move thunderbolt = new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special);
    private static readonly Pokemon pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, thunderbolt, PokemonType.Electric);
    private static readonly PokemonTeam team = new PokemonTeam("Ash's Team", pikachu);

    [Fact]
    public void AddPokemon_Success()
    {
        var user = new User();
        user.AddPokemon(pikachu);
        
        Assert.Contains(pikachu, user.PokemonList);
    }

    [Fact]
    public void RemovePokemon_Success()
    {
        var user = new User();
        user.AddPokemon(pikachu);
        
        user.RemovePokemon(0);
        
        Assert.DoesNotContain(pikachu, user.PokemonList);
    }

    [Fact]
    public void RemovePokemon_InvalidIndex_ThrowsException()
    {
        var user = new User();
        user.AddPokemon(pikachu);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => user.RemovePokemon(1));
    }

    [Fact]
    public void AddPokemonTeam_Success()
    {
        var user = new User();
        user.AddPokemonTeam(team);
        
        Assert.Contains(team, user.PokemonTeams);
    }

    [Fact]
    public void RemovePokemonTeam_Success()
    {
        var user = new User();
        user.AddPokemonTeam(team);
        
        user.RemovePokemonTeam(0);
        
        Assert.DoesNotContain(team, user.PokemonTeams);
    }

    [Fact]
    public void RemovePokemonTeam_InvalidIndex_ThrowsException()
    {
        var user = new User();
        user.AddPokemonTeam(team);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => user.RemovePokemonTeam(1));
    }

    [Fact]
    public void AddMove_Success()
    {
        var user = new User();
        user.AddMove(thunderbolt);
        
        Assert.Contains(thunderbolt, user.Moves);
    }

    [Fact]
    public void RemoveMove_Success()
    {
        var user = new User();
        user.AddMove(thunderbolt);
        
        user.RemoveMove(0);
        
        Assert.DoesNotContain(thunderbolt, user.Moves);
    }

    [Fact]
    public void RemoveMove_InvalidIndex_ThrowsException()
    {
        var user = new User();
        user.AddMove(thunderbolt);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => user.RemoveMove(1));
    }

    [Fact]
    public void ClearAllData_Success()
    {
        var user = new User();
        user.AddPokemon(pikachu);
        user.AddPokemonTeam(team);
        user.AddMove(thunderbolt);
        
        user.ClearAllData();
        
        Assert.Empty(user.PokemonList);
        Assert.Empty(user.PokemonTeams);
        Assert.Empty(user.Moves);
    }

    [Fact]
    public void CopyFromAnotherUser_Success()
    {
        var user1 = new User();
        user1.AddPokemon(pikachu);
        user1.AddPokemonTeam(team);
        user1.AddMove(thunderbolt);
        var user2 = new User();
        user2.CopyFrom(user1);
        Assert.Equal(user1.PokemonList, user2.PokemonList);
        Assert.Equal(user1.PokemonTeams, user2.PokemonTeams);
        Assert.Equal(user1.Moves, user2.Moves);
    }
}