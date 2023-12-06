#if UNITY_EDITOR
using GameSystems.Core;
using UnityEditor;

public static class LevelDataCreator
{
    [MenuItem("Tools/Create Level Data")]
    public static void CreateLevelData()
    {
        LevelData levelData = Util.LevelLoader.LoadLevels();

        // Save the LevelData instance as a ScriptableObject asset
        AssetDatabase.CreateAsset(levelData, "Assets/Resources/LevelData.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif