using System;

namespace PokemonBattleSimulator;

internal class BattlePokemonTeam
{
    public PokemonTeam _pokemonTeam;
    public string Name => _pokemonTeam.Name; // Team name from the PokemonTeam
    public BattlePokemon[] BattlePokemonList { get; private set; }

    private readonly AITeamStrategy _aiTeamStrategy;

    public bool AllPokemonFainted => Array.TrueForAll(BattlePokemonList, bp => bp == null || bp.Fainted);

    public BattlePokemonTeam(PokemonTeam pokemonTeam, AIStrategy aIStrategy, AITeamStrategy aITeamStrategy)
    {
        _pokemonTeam = pokemonTeam ?? throw new ArgumentNullException(nameof(pokemonTeam), "Pokemon team cannot be null.");

        BattlePokemonList = new BattlePokemon[PokemonTeam.MaxTeamSize];

        // Create BattlePokemon wrapper for each Pokemon in the team
        for (int i = 0; i < PokemonTeam.MaxTeamSize; i++)
        {
            if (_pokemonTeam.PokemonList[i] != null)
            {
                BattlePokemonList[i] = new BattlePokemon(_pokemonTeam.PokemonList[i], aIStrategy);
            }
        }

        _aiTeamStrategy = aITeamStrategy ?? throw new ArgumentNullException(nameof(aITeamStrategy), "AI team strategy cannot be null.");
    }

    public BattlePokemonTeam(BattlePokemonTeam other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other), "Other BattlePokemonTeam cannot be null.");

        _pokemonTeam = other._pokemonTeam; // Reference to the same Pokemon team
        BattlePokemonList = new BattlePokemon[PokemonTeam.MaxTeamSize];

        // Clone each BattlePokemon
        for (int i = 0; i < PokemonTeam.MaxTeamSize; i++)
        {
            if (other.BattlePokemonList[i] != null)
            {
                BattlePokemonList[i] = new BattlePokemon(other.BattlePokemonList[i]);
            }
        }

        _aiTeamStrategy = other._aiTeamStrategy; // Use the same AI team strategy
    }

    public BattlePokemon PickNextBattlePokemon(BattlePokemon opponent)
    {
        if (opponent == null) throw new ArgumentNullException(nameof(opponent), "Opponent cannot be null.");

        // Use the AI team strategy to pick the next BattlePokemon
        var selectedPokemon = _aiTeamStrategy(this, opponent);

        return selectedPokemon;
    }
}
