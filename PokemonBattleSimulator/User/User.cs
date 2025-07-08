using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokemonBattleSimulator;

/// <summary>
/// Represents a user in the Pokemon Battle Simulator, containing their Pokemon, teams, and moves.
/// </summary>
internal class User
{
    [JsonInclude]   // Needed as setter is private
    public List<Pokemon> PokemonList { get; private set; } = new List<Pokemon>();

    [JsonInclude]
    public List<PokemonTeam> PokemonTeams { get; private set; } = new List<PokemonTeam>();

    [JsonInclude]
    public List<Move> Moves { get; private set; } = new List<Move>();

    /// <summary>
    /// Adds a Pokemon to the user's collection.
    /// </summary>
    /// <param name="pokemon">Pokemon to add.</param>
    /// <exception cref="ArgumentNullException">Pokemon is null.</exception>
    public void AddPokemon(Pokemon pokemon)
    {
        if (pokemon == null) throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
        PokemonList.Add(pokemon);
    }

    /// <summary>
    /// Adds a Pokemon team to the user's collection.
    /// </summary>
    /// <param name="team">Pokemon team to add.</param>
    /// <exception cref="ArgumentNullException">Pokemon team is null.</exception>
    public void AddPokemonTeam(PokemonTeam team)
    {
        if (team == null) throw new ArgumentNullException(nameof(team), "Pokemon team cannot be null.");
        PokemonTeams.Add(team);
    }

    /// <summary>
    /// Adds a move to the user's collection.
    /// </summary>
    /// <param name="move">Move to add.</param>
    /// <exception cref="ArgumentNullException">Move is null.</exception>
    public void AddMove(Move move)
    {
        if (move == null) throw new ArgumentNullException(nameof(move), "Move cannot be null.");
        Moves.Add(move);
    }

    /// <summary>
    /// Removes a Pokemon from the user's collection by index.
    /// </summary>
    /// <param name="index">Index of the Pokemon to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Index is out of range.</exception>
    public void RemovePokemon(int index)
    {
        if (index < 0 || index >= PokemonList.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the Pokemon list.");
        }
        PokemonList.RemoveAt(index);
    }

    /// <summary>
    /// Removes a Pokemon team from the user's collection by index.
    /// </summary>
    /// <param name="index">Index of the Pokemon team to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Index is out of range.</exception>
    public void RemovePokemonTeam(int index)
    {
        if (index < 0 || index >= PokemonTeams.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the Pokemon teams list.");
        }
        PokemonTeams.RemoveAt(index);
    }

    /// <summary>
    /// Removes a move from the user's collection by index.
    /// </summary>
    /// <param name="index">Index of the move to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Index is out of range.</exception>
    public void RemoveMove(int index)
    {
        if (index < 0 || index >= Moves.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the Moves list.");
        }
        Moves.RemoveAt(index);
    }

    /// <summary>
    /// Clears all data from the user's collections, including Pokemon, teams, and moves.
    /// </summary>
    public void ClearAllData()
    {
        PokemonList.Clear();
        PokemonTeams.Clear();
        Moves.Clear();
    }

    /// <summary>
    /// Copies data from another user into this user instance.
    /// </summary>
    /// <param name="other">User instance to copy data from.</param>
    /// <exception cref="ArgumentNullException">User to copy from is null.</exception>
    public void CopyFrom(User other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other), "Other user cannot be null.");

        PokemonList = new List<Pokemon>(other.PokemonList);
        PokemonTeams = new List<PokemonTeam>(other.PokemonTeams);
        Moves = new List<Move>(other.Moves);
    }
}
