---
uid: user-docs
---

# User Documentation

Pokémon Battle Simulator is a CLI tool that allows users to define their own Moves, Pokémon, and Pokémon Teams, and then have them battle against each other to determine which one is better.

The application is navigated by typing commands when prompted. Entering invalid commands will prompt again.

### Main Menu

After starting the simulator, you will be presented with the main menu. Here, you can type `build` to go to the build menu, `battle` to go to the battle menu, or `exit` to exit the simulator.

### Build Menu

In the build menu, you can create and manage your Pokémon, Moves, and Teams.

Valid commands in the build menu:
- `back` - Go back to the main menu.
- `listM`, `listP`, `listT` - List all created Moves, Pokémon, or Teams respectively.
- `newM`, `newP`, `newT` - Create a new Move, Pokémon, or Team respectively.
- `defaults` - Load default Moves, Pokémon, and Teams. Keep in mind that this will overwrite any existing Moves, Pokémon, or Teams you have created.
- `save` - Save all created Moves, Pokémon, and Teams to a `user.json` file.
- `load` - Load Moves, Pokémon, and Teams from a `user.json` file.

When listing Moves, Pokémon or Teams, you will be presented with a paginated list. You can navigate it using `n` (next page), `p` (previous page), or `q` (back to the build menu). You can also type `delete (number)` to delete an item from the list.

When creating a new Move, Pokémon, or Team, you will be prompted to enter the required information. For Moves, this includes name, type, category, power, accuracy, and PP. For Pokémon, this includes name, first and second type, level, stats (HP, Attack, Defense, Special Attack, Special Defense, Speed), and moves. For Teams, you will need to provide a name and list pick Pokémon. Enter the information either directly or select from a paginated list. You may at any point type `abort` to cancel the creation process.

Saving and loading will store all data in a `user.json` stored in the same directory as the simulator executable. This file will be created if it does not exist, and will be overwritten if it does.

### Battle Menu

In the battle menu, you can start battles between your Pokemon and Teams.

Valid commands in the battle menu:
- `back` - Go back to the main menu.
- `battle`, `battleT` - Start a battle between two Pokémon or Teams respectively.
- `battleMany`, `battleTMany` - Start multiple battles between Pokémon or Teams respectively.

Starting a battle will prompt you to select the two Pokémon or teams you want to battle. You will also be able to select the Pokémon and Team strategies. After that a the whole battle will be simulated and all actions taken will be printed out. After the battle, you will be taken back to the battle menu.

When starting multiple battles, you will be prompted to select the Pokémon or Teams you want to battle. You can also select the number of battles to run. After all battles are completed, a summary of the results will be displayed. This way you can quickly determine which Pokémon or Team is better over many battles.