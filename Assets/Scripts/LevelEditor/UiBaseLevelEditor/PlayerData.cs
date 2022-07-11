using System.Collections.Generic;
using MHamidi;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class PlayerData
{
    public int characterSkin;
    public int currentLanguage;//we can Save the language for Localization and then Use a Lookup Table to Load the correct Text for each Element 
    public string saveName;//We Can Use this To Have Multiple Saves
    
    public bool soundSetting;//We Could use this for Setting
    public List<int> unlockedLevels;
    
    
    public PlayerData()
    {
        characterSkin=0;
        currentLanguage=0;//we can Save the language for Localization and then Use a Lookup Table to Load the correct Text for each Element 
        saveName="save";//We Can Use this To Have Multiple Saves
    
        soundSetting=true;//We Could use this for Setting
        unlockedLevels=new List<int>();    
    }
    
    public PlayerData(string responseBody)
    {
    
    }
    public PlayerData(JObject jObject)
    {

        characterSkin = Util.NullabelCaster.CastInt(jObject["characterSkin"]);
        currentLanguage = Util.NullabelCaster.CastInt(jObject["currentLanguage"]);
        saveName = Util.NullabelCaster.CastString(jObject["saveName"]);
        soundSetting = Util.NullabelCaster.CastBool(jObject["soundSetting"]);
        var unlocked = Util.NullabelCaster.CastJArray(jObject["unloackedLevels"]);
        unlockedLevels = new List<int>();
        if (unlocked.Count>0)
        {
            foreach (var item in unlocked )
            {
                unlockedLevels.Add(Util.NullabelCaster.CastInt(item));
            }    
        }
        




    }
}