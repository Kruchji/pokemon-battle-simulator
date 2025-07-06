using System;

namespace PokemonBattleSimulator;

// Pokemon team consists of 6 Pokemon
public class PokemonTeam
{
    public static readonly int MaxTeamSize = 6;
    public string Name { get; set; }
    public Pokemon[] PokemonList { get; private set; } = new Pokemon[MaxTeamSize];
    public PokemonTeam(string name, Pokemon firstPokemon)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Team name cannot be null.");
        if (firstPokemon == null)
        {
            throw new ArgumentNullException(nameof(firstPokemon), "First Pokemon cannot be null.");
        }
        PokemonList[0] = firstPokemon;
    }
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
        PokemonList[index] = pokemon;
    }
    public override string ToString()
    {
        return $"{Name} - {string.Join(", ", Array.ConvertAll(PokemonList, p => p?.Name ?? "Empty"))}";
    }
}
