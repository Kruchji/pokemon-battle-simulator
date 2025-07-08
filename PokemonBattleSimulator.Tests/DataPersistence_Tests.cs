namespace PokemonBattleSimulator.Tests;

using PokemonBattleSimulator;

public class DataPersistence_Tests
{
    private readonly IFileWrapper _fileWrapper;
    private readonly DataPersistence _dataPersistence;
    private static readonly string UserDataFile = "user.json";

    private class MockFileWrapper : IFileWrapper
    {
        private readonly Dictionary<string, string> _files = new();
        public bool Exists(string path) => _files.ContainsKey(path);
        public void WriteAllText(string path, string contents)
        {
            _files[path] = contents;
        }
        public string ReadAllText(string path)
        {
            return _files.TryGetValue(path, out var contents) ? contents : throw new FileNotFoundException($"File not found: {path}");
        }
    }
    public DataPersistence_Tests()
    {
        _fileWrapper = new MockFileWrapper();
        _dataPersistence = new DataPersistence(_fileWrapper);
    }

    private static readonly Move thunderbolt = new Move("Thunderbolt", 90, 100, 15, PokemonType.Electric, MoveCategory.Special);
    private static readonly Pokemon pikachu = new Pokemon("Pikachu", 10, 20, 30, 40, 50, 60, 70, thunderbolt, PokemonType.Electric);
    private static readonly PokemonTeam team = new PokemonTeam("Ash Team", pikachu);

    [Fact]
    public void SaveUserData_Success()
    {
        var user = new User();
        user.AddPokemon(pikachu);
        user.AddPokemonTeam(team);
        user.AddMove(thunderbolt);

        _dataPersistence.SerializeUserData(user);
        Assert.True(_fileWrapper.Exists(UserDataFile));

        string json = _fileWrapper.ReadAllText(UserDataFile);
        Assert.NotNull(json);
        Assert.Contains("Pikachu", json);
        Assert.Contains("Ash Team", json);
        Assert.Contains("Thunderbolt", json);
    }

    [Fact]
    public void LoadUserData_Success()
    {
        var user = new User();
        user.AddPokemon(pikachu);
        user.AddPokemonTeam(team);
        user.AddMove(thunderbolt);

        _dataPersistence.SerializeUserData(user);

        var loadedUser = new User();
        _dataPersistence.DeserializeUserData(loadedUser);
    
        Assert.Single(loadedUser.PokemonList);
        Assert.Equal("Pikachu", loadedUser.PokemonList[0].Name);
        Assert.Single(loadedUser.PokemonTeams);
        Assert.Equal("Ash Team", loadedUser.PokemonTeams[0].Name);
        Assert.Single(loadedUser.Moves);
        Assert.Equal("Thunderbolt", loadedUser.Moves[0].Name);
    }
}