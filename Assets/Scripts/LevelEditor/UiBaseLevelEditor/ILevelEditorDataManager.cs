namespace GameSystems.Core
{
    public interface ILevelEditorDataManager
    {
        void SaveLevel(Level level);
        void UpdateLevel(Level level);
        Level LoadLevel(int levelId);
        void AddNewLevel(Level level);
        int FindLevelIndex(int levelNumber);
        // Additional methods for level data management...
    }
}