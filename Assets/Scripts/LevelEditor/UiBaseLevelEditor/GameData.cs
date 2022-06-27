using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class GameData
{
    
    
    
    public List<Level> levels;

    public GameData()
    {
        levels=new List<Level>();
    }
    public GameData(string responseBody)
    {
           
    }
    public GameData(JObject jObject)
    {

        var data = (JArray)jObject["levels"];
        foreach (var token in data)
        {
            var level = new Level(token);
            levels.Add(level);

        }
    }
    
}