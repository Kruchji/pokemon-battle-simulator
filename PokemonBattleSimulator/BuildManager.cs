using System;

namespace PokemonBattleSimulator;

internal static class BuildManager
{
    public static void CreatePokemon(User user)
    {
        // Guides user to enter all details and constructs a new Pokemon
    }

    public static void LoadDefaults(User user)
    {
        user.ClearAllData();

        // TODO: Create some default Pokemon (with real stats) and Moves (and add to Pokemon) and add them to the user
        var pikachu = new Pokemon("Pikachu", 35, 55, 40, 50, 50, 90, 10, PokemonType.Electric);
        var charmander = new Pokemon("Charmander", 39, 52, 43, 60, 50, 65, 10, PokemonType.Fire);
        var bulbasaur = new Pokemon("Bulbasaur", 45, 49, 49, 65, 65, 45, 10, PokemonType.Grass);

        var thunderbolt = new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special);
        var flamethrower = new Move("Flamethrower", 90, 100, 15, PokemonType.Fire, MoveCategory.Special);
        var vineWhip = new Move("Vine Whip", 45, 100, 25, PokemonType.Grass, MoveCategory.Physical);
        var scratch = new Move("Scratch", 40, 100, 35, PokemonType.Normal, MoveCategory.Physical);
        var tackle = new Move("Tackle", 40, 100, 35, PokemonType.Normal, MoveCategory.Physical);

        pikachu.SetMove(0, thunderbolt);
        pikachu.SetMove(1, tackle);
        pikachu.SetMove(2, scratch);

        charmander.SetMove(0, flamethrower);
        charmander.SetMove(1, scratch);
        charmander.SetMove(2, tackle);

        bulbasaur.SetMove(0, vineWhip);
        bulbasaur.SetMove(1, tackle);

        var starterTeam = new PokemonTeam("Starter Team");
        starterTeam.AddPokemon(0, pikachu); 
        starterTeam.AddPokemon(1, charmander);
        starterTeam.AddPokemon(2, bulbasaur);

        user.AddPokemon(pikachu);
        user.AddPokemon(charmander);
        user.AddPokemon(bulbasaur);

        user.AddPokemonTeam(starterTeam);

        user.AddMove(thunderbolt);
        user.AddMove(flamethrower);
        user.AddMove(vineWhip);
        user.AddMove(scratch);
        user.AddMove(tackle);
    }
}
