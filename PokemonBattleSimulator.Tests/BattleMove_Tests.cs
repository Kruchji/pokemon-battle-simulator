namespace PokemonBattleSimulator.Tests;

using PokemonBattleSimulator;

public class BattleMove_Tests
{
    private static readonly Move thunderbolt = new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special);

    [Fact]
    public void CreateBattleMove_SetsCurrentPPCorrectly()
    {

        var battleMove = new BattleMove(thunderbolt);

        Assert.Equal(thunderbolt.PP, battleMove.CurrentPP);
    }

    [Fact]
    public void BattleMove_UseDecreasesCurrentPP()
    {
        var battleMove = new BattleMove(thunderbolt);
        int initialPP = battleMove.CurrentPP;
        battleMove.UseMove();
        Assert.Equal(initialPP - 1, battleMove.CurrentPP);
    }

    [Fact]
    public void BattleMove_UseThrowsExceptionWhenPPIsZero()
    {
        var battleMove = new BattleMove(thunderbolt);
        for (int i = 0; i < thunderbolt.PP; i++)
        {
            battleMove.UseMove(); // Deplete PP
        }
        Assert.Throws<InvalidOperationException>(() => battleMove.UseMove());
    }

    [Fact]
    public void CreateBattleMoveWithNullMove_SetsCurrentPPToZero()
    {
        var battleMove = new BattleMove((Move)null!);
        Assert.Equal(0, battleMove.CurrentPP);
    }
}