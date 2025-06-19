using System;

namespace PokemonBattleSimulator;

// Pokemon team consists of 6 Pokemon
internal class PokemonTeam
{
    public static readonly int MaxTeamSize = 6;
    public string Name { get; set; }
    public Pokemon[] PokemonList { get; private set; } = new Pokemon[MaxTeamSize];
    public PokemonTeam(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Team name cannot be null.");
    }
    public void AddPokemon(Pokemon pokemon, int index)
    {
        if (index < 0 || index >= MaxTeamSize)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 5.");
        }
        if (pokemon == null)
        {
            throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
        }
        PokemonList[index] = pokemon;
    }
    public override string ToString()
    {
        return $"{Name} - {string.Join(", ", Array.ConvertAll(PokemonList, p => p?.Name ?? "Empty"))}";
    }
}
