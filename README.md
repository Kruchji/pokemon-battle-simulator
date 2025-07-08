## Basic Pokémon Battle Simulator

This CLI C# (.NET 8) simulator allows user to define their own Pokémon (stats, moves, etc.) and Pokémon Teams and then have them battle against each other to determine which one is better. Simulator can also run many battles simultaneously and display final results.

All project files are located in the [PokemonBattleSimulator](PokemonBattleSimulator) directory and tests can be found in the [PokemonBattleSimulator](PokemonBattleSimulator) directory.

## Documentation

Documentation is located in the [docs](docs) folder. To generate the `docfx` documentation, run the following command in the [docs](docs) directory:
```bash
docfx docfx.json --serve
```

This will generate both the user and developer (+ API) documentation and serve it on `http://localhost:8080`.

PDF version of the user and developer documentation can also be found in the [docs/documentation-pdf](docs/documentation-pdf/documentation.pdf) directory.