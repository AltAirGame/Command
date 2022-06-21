using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class LevelCollection
{
    public List<Level> levels;

    public LevelCollection()
    {
        levels=new List<Level>();
    }
    public LevelCollection(string responseBody)
    {
           
    }
    public LevelCollection(JObject jObject)
    {

        var data = (JArray)jObject["levels"];
        foreach (var token in data)
        {
            var level = new Level(token);
            levels.Add(level);

        }
    }
    
}