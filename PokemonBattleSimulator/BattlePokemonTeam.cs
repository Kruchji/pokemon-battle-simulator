using System;

namespace PokemonBattleSimulator;

internal class BattlePokemonTeam
{
    public PokemonTeam _pokemonTeam;
    public BattlePokemon[] BattlePokemonList { get; private set; }

    public BattlePokemonTeam(PokemonTeam pokemonTeam)
    {
        _pokemonTeam = pokemonTeam ?? throw new ArgumentNullException(nameof(pokemonTeam), "Pokemon team cannot be null.");

        BattlePokemonList = new BattlePokemon[PokemonTeam.MaxTeamSize];

        // Create BattlePokemon wrapper for each Pokemon in the team
        for (int i = 0; i < PokemonTeam.MaxTeamSize; i++)
        {
            if (_pokemonTeam.PokemonList[i] != null)
            {
                BattlePokemonList[i] = new BattlePokemon(_pokemonTeam.PokemonList[i]);
            }
        }
    }
}
