namespace PokemonBattleSimulator;

internal class Program
{
    static void Main(string[] args)     // TODO: Take file to load Pokemon data from command line arguments
    {
        // TODO: Implement main logic for the Pokemon Battle Simulator.

        var user = new User();

        var mainController = new MainController();
        mainController.Run(user);
    }
}
