using System;
using System.Collections;
using System.Collections.Generic;

namespace PokemonBattleSimulator;

internal class User
{
    public List<Pokemon> PokemonList { get; private set; } = new List<Pokemon>();
    public List<PokemonTeam> PokemonTeams { get; private set; } = new List<PokemonTeam>();

    public void AddPokemon(Pokemon pokemon)
    {
        if (pokemon == null) throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
        PokemonList.Add(pokemon);
    }

    public void AddPokemonTeam(PokemonTeam team)
    {
        if (team == null) throw new ArgumentNullException(nameof(team), "Pokemon team cannot be null.");
        PokemonTeams.Add(team);
    }
}
