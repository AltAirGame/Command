namespace GameSystems.Core
{
    public class LevelEditorDataManager : ILevelEditorDataManager
    {
        private LevelData levelData; // Assuming LevelData is a class that stores level information
        private IDataManagementService dataManagementService; // Assuming this service handles data persistence

        public LevelEditorDataManager(LevelData levelData, IDataManagementService dataManagementService)
        {
            this.levelData = levelData;
            this.dataManagementService = dataManagementService;
        }

        public void SaveLevel(Level level)
        {
            UpdateLevelInScriptableObject(level);
            // Other save logic...
        }

        public void UpdateLevel(Level level)
        {
            int index = FindLevelIndex(level.number);
            if (index >= 0)
            {
                levelData.levels[index] = level;
            }
            else
            {
                // Add new level logic...
            }
            // Additional update logic...
        }

        public Level LoadLevel(int levelId)
        {
            // Logic to load a level based on levelId
            return dataManagementService.LevelData.GetLevel(levelId);
        }

        public void AddNewLevel(Level level)
        {
            // Logic to add a new level
            dataManagementService.AddLevel(level);
        }

        public int FindLevelIndex(int levelNumber)
        {
            for (int i = 0; i < levelData.levels.Count; i++)
            {
                if (levelData.levels[i].number == levelNumber)
                {
                    return i;
                }
            }
            return -1; // Not found
        }

        private void UpdateLevelInScriptableObject(Level level)
        {
            // Logic to update the level in the ScriptableObject
            // For example, marking the object as dirty in Unity Editor
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(levelData);
#endif
        }

        // Additional methods or helper functions as required...
    }
}