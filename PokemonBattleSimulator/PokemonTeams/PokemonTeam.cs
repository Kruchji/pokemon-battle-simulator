using System;
using System.Text.Json.Serialization;

namespace PokemonBattleSimulator;

/// <summary>
/// Represents a team of Pokemon, allowing for a maximum of 6 Pokemon.
/// </summary>
internal class PokemonTeam
{
    /// <summary>
    /// Maximum number of Pokemon in a team.
    /// </summary>
    public static readonly int MaxTeamSize = 6;  // Pokemon team consists of 6 Pokemon
    public string Name { get; set; }
    public Pokemon[] PokemonList { get; private set; } = new Pokemon[MaxTeamSize];

    /// <summary>
    /// Initializes a new instance of the <see cref="PokemonTeam"/> class with a name and the first Pokemon.
    /// </summary>
    /// <param name="name">Name of the Pokemon team.</param>
    /// <param name="firstPokemon">First Pokemon in the team.</param>
    /// <exception cref="ArgumentNullException">Team name or first Pokemon is null.</exception>
    public PokemonTeam(string name, Pokemon firstPokemon)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Team name cannot be null.");
        PokemonList[0] = firstPokemon ?? throw new ArgumentNullException(nameof(firstPokemon), "First Pokemon cannot be null.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PokemonTeam"/> class with a name and a list of Pokemon. Used for deserialization.
    /// </summary>
    /// <param name="name">Name of the Pokemon team.</param>
    /// <param name="pokemonList">Array of Pokemon in the team. Must contain exactly 6 Pokemon.</param>
    /// <exception cref="ArgumentNullException">Team name or first Pokemon in the list is null.</exception>
    /// <exception cref="ArgumentException">Array does not contain exactly 6 Pokemon.</exception>
    [JsonConstructor] // Needed for deserialization
    public PokemonTeam(string name, Pokemon[] pokemonList)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Team name cannot be null.");
        if (pokemonList == null || pokemonList.Length != MaxTeamSize)
        {
            throw new ArgumentException($"Pokemon list must contain exactly {MaxTeamSize} Pokemon.", nameof(pokemonList));
        }
        for (int i = 0; i < MaxTeamSize; i++)
        {
            if (i == 0 && pokemonList[i] == null)
            {
                throw new ArgumentNullException(nameof(pokemonList), "First Pokemon cannot be null.");
            }
            PokemonList[i] = pokemonList[i];
        }
    }

    /// <summary>
    /// Adds a Pokemon to the team at the specified index.
    /// </summary>
    /// <param name="index">Index to add the Pokemon at (0-5).</param>
    /// <param name="pokemon">Pokemon to add.</param>
    /// <exception cref="ArgumentOutOfRangeException">Index is out of range (0-5).</exception>
    /// <exception cref="ArgumentNullException">Pokemon is null when trying to add at index 0.</exception>
    public void AddPokemon(int index, Pokemon pokemon)
    {
        if (index < 0 || index >= MaxTeamSize)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 5.");
        }
        if (pokemon == null && index == 0)
        {
            throw new ArgumentNullException(nameof(pokemon), "First Pokemon cannot be null.");
        }
        PokemonList[index] = pokemon!;
    }

    /// <summary>
    /// Returns a string representation of the Pokemon team, including the team name and the names of the Pokemon in the team. 
    /// </summary>
    /// <returns>The string representation of the Pokemon team.</returns>
    public override string ToString()
    {
        return $"{Name} - {string.Join(", ", Array.ConvertAll(PokemonList, p => p?.Name ?? "Empty"))}";
    }
}
