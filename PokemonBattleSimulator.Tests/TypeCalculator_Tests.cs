namespace PokemonBattleSimulator.Tests;

using PokemonBattleSimulator;

public class TypeCalculator_Tests
{
    private static readonly Move thunderbolt = new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special);
    private static readonly Move vineWhip = new Move("Vine Whip", 90, 100, 15, PokemonType.Grass, MoveCategory.Physical);
    private static readonly Move ember = new Move("Ember", 90, 100, 15, PokemonType.Fire, MoveCategory.Special);
    private static readonly Pokemon bulbasaur = new Pokemon("Bulbasaur", 10, 20, 30, 40, 50, 60, 70, vineWhip, PokemonType.Grass);
    private static readonly Pokemon charmander = new Pokemon("Charmander", 10, 20, 30, 40, 50, 60, 70, ember, PokemonType.Fire);

    [Fact]
    public void EmberVsBulbasaur_ReturnsSuperEffective()
    {
        double effectiveness = TypeCalculator.GetMoveEffectiveness(ember, bulbasaur);
        Assert.Equal(2.0, effectiveness); // Fire vs Grass is super effective
    }

    [Fact]
    public void ThunderboltVsCharmander_ReturnsNormallyEffective()
    {
        double effectiveness = TypeCalculator.GetMoveEffectiveness(thunderbolt, charmander);
        Assert.Equal(1.0, effectiveness); // Electric vs Fire is normally effective
    }

    [Fact]
    public void VineWhipVsCharmander_ReturnsNotVeryEffective()
    {
        double effectiveness = TypeCalculator.GetMoveEffectiveness(vineWhip, charmander);
        Assert.Equal(0.5, effectiveness); // Grass vs Fire is not very effective
    }
}