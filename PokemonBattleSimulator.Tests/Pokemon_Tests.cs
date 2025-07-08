namespace PokemonBattleSimulator.Tests;

using PokemonBattleSimulator;

public class Pokemon_Tests
{
    [Fact]
    public void CreatePikachu_Success()
    {
        var pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric);

        Assert.NotNull(pikachu);
        Assert.Equal("Pikachu", pikachu.Name);
        Assert.Equal(10, pikachu.Level);
        Assert.Equal(20, pikachu.Health);
        Assert.Equal(30, pikachu.Attack);
        Assert.Equal(40, pikachu.Defense);
        Assert.Equal(50, pikachu.Speed);
        Assert.Equal(60, pikachu.SpecialAttack);
        Assert.Equal(70, pikachu.SpecialDefense);
        Assert.Equal(PokemonType.Electric, pikachu.FirstType);
        Assert.Null(pikachu.SecondType);
    }

    [Fact]
    public void CreatePokemonWithTwoTypes_Success()
    {
        var pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric, PokemonType.Flying);
        Assert.NotNull(pikachu);
        Assert.Equal("Pikachu", pikachu.Name);
        Assert.Equal(PokemonType.Electric, pikachu.FirstType);
        Assert.Equal(PokemonType.Flying, pikachu.SecondType);
    }

    [Fact]
    public void AssignMoreMovesToPokemon_Success()
    {
        var pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric);
        
        var move2 = new Move("Quick Attack", 40, 100, 30, PokemonType.Normal, MoveCategory.Physical);
        pikachu.SetMove(1, move2);
        
        Assert.Equal(move2, pikachu.Moves[1]);
    }

    [Fact]
    public void AssignMoveAtInvalidPositionToPokemon_ThrowsException()
    {
        var pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => pikachu.SetMove(4, new Move("Invalid Move", 50, 100, 20, PokemonType.Normal, MoveCategory.Physical)));
    }

    [Fact]
    public void AssignNullFirstMoveToPokemon_ThrowsException()
    {
        var pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric);
        
        Assert.Throws<ArgumentNullException>(() => pikachu.SetMove(0, null!));
    }

    [Fact]
    public void AssignNullMoveCorrectly_Success()
    {
        var pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special), PokemonType.Electric);

        var move2 = new Move("Quick Attack", 40, 100, 30, PokemonType.Normal, MoveCategory.Physical);
        pikachu.SetMove(1, move2);

        pikachu.SetMove(1, null!); // Assigning null to a non-first move should not throw an exception

        Assert.Null(pikachu.Moves[1]);
    }
}