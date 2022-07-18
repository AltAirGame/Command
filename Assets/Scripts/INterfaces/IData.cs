namespace MHamidi
{
    public interface IData
    {
        public PlayerData playerData { get; set; }
        public GameData gameData { get; set; }
        
        public void AddTOLevels(Level level);
        public void EditPlayedData(PlayerData playerData);

    }
}