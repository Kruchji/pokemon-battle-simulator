using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokemonBattleSimulator;

internal class User
{
    [JsonInclude]   // Needed as setter is private
    public List<Pokemon> PokemonList { get; private set; } = new List<Pokemon>();

    [JsonInclude]
    public List<PokemonTeam> PokemonTeams { get; private set; } = new List<PokemonTeam>();

    [JsonInclude]
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

    public void RemovePokemon(int index)
    {
        if (index < 0 || index >= PokemonList.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the Pokemon list.");
        }
        PokemonList.RemoveAt(index);
    }

    public void RemovePokemonTeam(int index)
    {
        if (index < 0 || index >= PokemonTeams.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the Pokemon teams list.");
        }
        PokemonTeams.RemoveAt(index);
    }

    public void RemoveMove(int index)
    {
        if (index < 0 || index >= Moves.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the Moves list.");
        }
        Moves.RemoveAt(index);
    }

    public void ClearAllData()
    {
        PokemonList.Clear();
        PokemonTeams.Clear();
        Moves.Clear();
    }

    public void CopyFrom(User other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other), "Other user cannot be null.");

        PokemonList = new List<Pokemon>(other.PokemonList);
        PokemonTeams = new List<PokemonTeam>(other.PokemonTeams);
        Moves = new List<Move>(other.Moves);
    }
}
