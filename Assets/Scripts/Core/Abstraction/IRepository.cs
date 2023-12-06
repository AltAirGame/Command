namespace GameSystems.Core
{
    public interface IRepository
    {
        void InitializeServices();
        RepositoryResponse<LevelData> LoadGameData(string gameDataFilename);
        RepositoryResponse<PlayerData> LoadPlayerData(string playerDataFilename);
        RepositoryResponse<LevelData> SaveGameData(string gameDataFilename, LevelData gameData);
        RepositoryResponse<PlayerData> SavePlayerData(string playerDataFilename, PlayerData playerData);
        RepositoryResponse<T> LoadData<T>(string saveDestination) where T : new();
        RepositoryResponse<T> SaveData<T>(string saveDestination, T data);
    }
}