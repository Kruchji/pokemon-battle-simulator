using System;

namespace PokemonBattleSimulator;

/// <summary>
/// Wrapper for a Pokemon team used in battle, tracking its list of BattlePokemon and AI strategy.
/// </summary>
internal class BattlePokemonTeam
{
    public PokemonTeam _pokemonTeam;
    public string Name => _pokemonTeam.Name; // Team name from the PokemonTeam
    public BattlePokemon[] BattlePokemonList { get; private set; }

    private readonly AITeamStrategy _aiTeamStrategy;

    public bool AllPokemonFainted => Array.TrueForAll(BattlePokemonList, bp => bp == null || bp.Fainted);

    /// <summary>
    /// Initializes a new instance of the <see cref="BattlePokemonTeam"/> class with a Pokemon team and AI strategy.
    /// </summary>
    /// <param name="pokemonTeam">Pokemon team to wrap.</param>
    /// <param name="aIStrategy">AI strategy for Pokemon in the team.</param>
    /// <param name="aITeamStrategy">AI team strategy for selecting the next BattlePokemon.</param>
    /// <exception cref="ArgumentNullException">Pokemon team or AI strategy is null.</exception>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="BattlePokemonTeam"/> class by copying another BattlePokemonTeam.
    /// </summary>
    /// <param name="other">BattlePokemonTeam to copy from.</param>
    /// <exception cref="ArgumentNullException">BattlePokemonTeam to copy from is null.</exception>
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

    /// <summary>
    /// Selects the next BattlePokemon to use against the opponent based on the AI team strategy.
    /// </summary>
    /// <param name="opponent">Opponent's BattlePokemon to consider when selecting the next BattlePokemon.</param>
    /// <returns>BattlePokemon selected by the AI team strategy.</returns>
    /// <exception cref="ArgumentNullException">Opponent BattlePokemon is null.</exception>
    public BattlePokemon PickNextBattlePokemon(BattlePokemon opponent)
    {
        if (opponent == null) throw new ArgumentNullException(nameof(opponent), "Opponent cannot be null.");

        // Use the AI team strategy to pick the next BattlePokemon
        var selectedPokemon = _aiTeamStrategy(this, opponent);

        return selectedPokemon;
    }
}
