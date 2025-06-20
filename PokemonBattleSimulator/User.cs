using System;
using System.Collections;
using System.Collections.Generic;

namespace PokemonBattleSimulator;

internal class User
{
    public List<Pokemon> PokemonList { get; private set; } = new List<Pokemon>();
    public List<PokemonTeam> PokemonTeams { get; private set; } = new List<PokemonTeam>();

    public List<Move> Moves { get; private set; } = new List<Move>();

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

    public void AddMove(Move move)
    {
        if (move == null) throw new ArgumentNullException(nameof(move), "Move cannot be null.");
        Moves.Add(move);
    }

    public void ClearAllData()
    {
        PokemonList.Clear();
        PokemonTeams.Clear();
        Moves.Clear();
    }
}
