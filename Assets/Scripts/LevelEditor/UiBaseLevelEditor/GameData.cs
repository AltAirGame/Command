using System;
using System.Collections.Generic;
using MHamidi;
using Newtonsoft.Json.Linq;
using UnityEngine;

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
        
        
        if (jObject is null)
        {
            Debug.Log($" Data is Null");
            levels = new List<Level>();
            
        }
        else
        {               
            var data = Util.NullabelCaster.CastJArray((JArray)jObject["levels"]);
            foreach (var token in data)
            {
                var level = new Level(token);
                levels.Add(level);

            }
        }
    }
    
}