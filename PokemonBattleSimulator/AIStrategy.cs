using System;

namespace PokemonBattleSimulator;

internal delegate void AIStrategy(BattlePokemon ownPokemon, BattlePokemon opponentPokemon);
internal delegate void AITeamStrategy(BattlePokemonTeam ownPokemonTeam, BattlePokemonTeam opponentPokemonTeam);
