namespace GameSystems.Core
{
    public interface IDataManagementService
    {
        PlayerData PlayerData { get; }
        
        public LevelData LevelData
        {
            get;
        }

        void AddLevel(Level level);
        void SaveData();
        void EditPlayerData(PlayerData data);
        void LoadGameData();
        void LoadPlayerData();

    }
}