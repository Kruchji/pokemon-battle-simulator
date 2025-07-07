namespace PokemonBattleSimulator;

internal class Program
{
    static void Main(string[] args)
    {
        // Create a new user storing all Moves, Pokemon and Teams
        var user = new User();

        // Launch main menu
        var mainController = new MainController();
        mainController.Run(user);
    }
}
