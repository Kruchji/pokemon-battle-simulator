---
uid: dev-docs
---

# Developer Documentation

Application files are separated into several directories, based on their purpose. The entrypoint of the application is the <xref:PokemonBattleSimulator.Program> class, which initializes the user and starts the main menu.

There is also `AssemblyInfo.cs` file, which enables tests to access internal members of the application. Tests are then located in the `PokemonBattleSimulator.Tests` directory.

### User Interface

User navigation in the application is achieved using three controllers: <xref:PokemonBattleSimulator.MainController>, <xref:PokemonBattleSimulator.BuildController>, and <xref:PokemonBattleSimulator.BattleController>. Each controller handles user input and manages the flow of the application.

Each menu (and other components that write to the console) print with a starting prefix. This is achieved using the <xref:PokemonBattleSimulator.PrefixedConsole> class, which provides methods for writing to the console with a specific prefix. This allows for consistent and easy formatting across the application. (there is also <xref:PokemonBattleSimulator.IPrefixedConsole> for potential mocking).

### Data Management

User is able to define their own Moves, Pokémon, and Teams. All of them are stored inside Lists in the <xref:PokemonBattleSimulator.User> class. User is then passed between controllers and in arguments to get and store data.

Moves, Pokémon and Teams each have their own classes: <xref:PokemonBattleSimulator.Move>, <xref:PokemonBattleSimulator.Pokemon>, and <xref:PokemonBattleSimulator.PokemonTeam> respectively. Each of them store their specific stats and properties (e.g. Pokemon stores array of Moves).

The created Moves, Pokémon and Teams change only in the build phase. That is why each of them has a battle wrapper which tracks their state when in battle. These are <xref:PokemonBattleSimulator.BattleMove>, <xref:PokemonBattleSimulator.BattlePokemon>, and <xref:PokemonBattleSimulator.BattlePokemonTeam> respectively. These are especially useful if we want to run multiple battles simultaneously, as they allow us to keep track of the state of each Pokémon and Team during the battle (with no conflict).

User is also able to save and load their data to/from a `user.json` file. This is achieved using the <xref:PokemonBattleSimulator.DataPersistence> class, which handles serialization and deserialization of the user data. It also utilizes the <xref:PokemonBattleSimulator.FileWrapper> class and <xref:PokemonBattleSimulator.IFileWrapper> interface which provides an abstraction over file operations, allowing for easier testing and mocking.

### Pokemon and Move Types

Pokemon types (such as Fire, Water, etc.) and Move categories (such as Physical, Special, etc.) are defined in the <xref:PokemonBattleSimulator.PokemonType> and <xref:PokemonBattleSimulator.MoveCategory> enums respectively.

The application also contains a <xref:PokemonBattleSimulator.TypeCalculator> class, which provides methods for calculating move effectiveness in battles base on type. It utilizes a type chart that it initializes in the class constructor.

There is also an extra move defined in the <xref:PokemonBattleSimulator.FallbackMove> class. This move is used when all of the Pokémon's moves are unavailable (e.g. due to PP being 0). (it is the equivalent of Struggle in the Pokémon games).

### Strategies

When starting a battle, user can select strategies for Pokémon and Teams. These strategies are defined as methods in the <xref:PokemonBattleSimulator.AIStrategies> and <xref:PokemonBattleSimulator.AITeamStrategies> classes. These methods correspond to their respective delegates: <xref:PokemonBattleSimulator.AIStrategy> and <xref:PokemonBattleSimulator.AITeamStrategy>. Strategy for Pokémon picks a Move to use next, while Team strategy picks a Pokémon to use next in the battle.

When prompting user for strategies, the <xref:PokemonBattleSimulator.StrategyManager> class is used. It uses reflection to find all methods that match the strategy delegates and then provides them to the user as options. This allows for easy addition of new strategies without changing the code.

### Build Phase

User navigation in the build phase is handled by the <xref:PokemonBattleSimulator.BuildController> class. It then utilizes the <xref:PokemonBattleSimulator.BuildManager> class that provides methods for creating Moves, Pokémon, and Teams or loading default data.

### Battle Phase

User navigation in the build phase is handled by the <xref:PokemonBattleSimulator.BattleController> class. It then utilizes the <xref:PokemonBattleSimulator.BattleManager> to allow user to select Pokémon or Team with strategies.

After user makes their selection, the <xref:PokemonBattleSimulator.Battle> class is used to run the battle. It provides methods for simulating (single or many) battles between two Pokémon or Teams. It does also utilize the <xref:PokemonBattleSimulator.DamageCalculator> class which stores logic for calculating damage dealt by moves based on Pokémon stats and types.

Multiple battles are run simultaneously in parallel and then atomically increment the win count for each Pokémon or Team.

Turn and battle results are then indicated using the <xref:PokemonBattleSimulator.TurnResult> and <xref:PokemonBattleSimulator.BattleResult> enums.

### Helper Classes

There are also some other helper classes:
- <xref:PokemonBattleSimulator.PaginatedLists> provides methods for displaying paginated lists to user.
- <xref:PokemonBattleSimulator.Prompts> provides methods for prompting user for input.
- <xref:PokemonBattleSimulator.Parsers> provides methods returning parsers.