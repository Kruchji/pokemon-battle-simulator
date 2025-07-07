namespace PokemonBattleSimulator.Tests;

using PokemonBattleSimulator;

// TODO: Test the fallback move behavior
// TODO: Test type effectiveness calculations
// TODO: Test DataPersistence by mocking file operations

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
}