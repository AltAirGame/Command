using Newtonsoft.Json.Linq;

[System.Serializable]
public class PlayerData
{
    public int characterSkin;
    public string saveName;
    public bool soundSetting;
    public int[] unlockedLevels;
    public LevelCollection levels;

    public PlayerData()
    {
        
    }
    public PlayerData(string responseBody)
    {
        
    }
    public PlayerData(JObject jObject)
    {
       
    }
}