using System.Collections.Generic;
using MHamidi;
using Newtonsoft.Json.Linq;


[System.Serializable]
public class Level
{
    public string name;
    public List<int> AvailableCommand = new List<int>();
    public int maxBufferSize;
    public int maxP1Size;
    public int maxP2Size;

    public List<int> levelBufferSize;
    public List<int> p1BufferSize;
    public List<int> p2BufferSize;
    public int[,] LevelLayout;


    public Level()
    {
    }

    public Level(LevelData levelData)
    {
    }

    public Level(JToken token)
    {
        name = Util.NullabelCaster.CastString(token["name"]);
        maxBufferSize = Util.NullabelCaster.CastInt(token["LevelBufferSize"]);
        maxP1Size = Util.NullabelCaster.CastInt(token["LevelBufferSize"]);
        maxP2Size = Util.NullabelCaster.CastInt(token["LevelBufferSize"]);


        var first = (JArray)token["LevelLayout"];
        var second = (JArray)first[0];
        var twoDimensionalJArray = new JArray[first.Count, second.Count];
        for (var i = 0; i < first.Count; i++)
        {
            for (int j = 0; j < second.Count; j++)
            {
                var s = (JArray)first[i];
                twoDimensionalJArray[i, j] = (JArray)s[j];
            }
        }

        for (int i = 0; i < first.Count; i++)
        {
            for (int j = 0; j < second.Count; j++)
            {
                LevelLayout[i, j] = Util.NullabelCaster.CastInt(twoDimensionalJArray[i, j]);
            }
        }
    }

    public Level(List<int> availableCommand, int[,] levelLayout, int bufferSize, int p1BufferSize, int p2BufferSize)
    {
        AvailableCommand = availableCommand;
    }

    public Level(string name, List<int> availableCommand, int[,] levelLayout, int bufferSize, int p1BufferSize,
        int p2BufferSize)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            this.name = $"unknown Level";
        }
        else
        {
            this.name = name;
        }

        this.LevelLayout = levelLayout;
    }


    public Level(List<int> availableCommand, ICellEditor[,] levelLayout, int mainBufferSize, int p1BufferSize,
        int p2BufferSize)
    {
    }
}

//this Data Class is Exist In the cuz of none Serialization of abstract classes 
[System.Serializable]
public class LevelData
{
    public string name;
    public List<int> avilableCommand = new List<int>();
    public int levelBufferSize;
    public int p1BufferSize;
    public int p2BufferSize;
    public int[,] levelLayout;

    public LevelData(JToken token)
    {
        name = Util.NullabelCaster.CastString(token["name"]);
        var levelBufferSize = Util.NullabelCaster.CastInt(token["LevelBufferSize"]);
        var p1BufferSize = Util.NullabelCaster.CastInt(token["LevelBufferSize"]);
        var p2BufferSize = Util.NullabelCaster.CastInt(token["LevelBufferSize"]);

        var first = (JArray)token["LevelLayout"];
        var second = (JArray)first[0];
        var twoDimensionalJarray = new JArray[first.Count, second.Count];
        for (int i = 0; i < first.Count; i++)
        {
            for (int j = 0; j < second.Count; j++)
            {
                var s = (JArray)first[i];
                twoDimensionalJarray[i, j] = (JArray)s[j];
            }
        }

        for (int i = 0; i < first.Count; i++)
        {
            for (int j = 0; j < second.Count; j++)
            {
                levelLayout[i, j] = Util.NullabelCaster.CastInt(twoDimensionalJarray[i, j]);
            }
        }
    }

    public LevelData()
    {
    }
}