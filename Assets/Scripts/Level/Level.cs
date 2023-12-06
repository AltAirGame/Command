using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace GameSystems.Core
{
    public enum PlayerDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    [System.Serializable]
    public class Level
    {
        public int width = 8;
        public int height = 8;
        public int number;
        public int startX = 0;
        public int startY = 0;
        public List<string> AvailableCommand = new List<string>();
        public int maxBufferSize;
        public int maxP1Size;
        public int maxP2Size;
        public Vector2Int Start;
        public PlayerDirection direction;
        public List<List<CellLayout>> LevelLayout;

        public Level()
        {
        }
        public Level(JToken token)
        {
            direction = (PlayerDirection) Util.NullableCaster.CastInt(token["direction"]);
            number = Util.NullableCaster.CastInt(token["number"]);
            startX = Util.NullableCaster.CastInt(token["startX"]);
            startY = Util.NullableCaster.CastInt(token["startY"]);
            maxBufferSize = Util.NullableCaster.CastInt(token["maxBufferSize"]);
            maxP1Size = Util.NullableCaster.CastInt(token["maxP1Size"]);
            maxP2Size = Util.NullableCaster.CastInt(token["maxP2Size"]);

            var available = Util.NullableCaster.CastJArray(token["AvailableCommand"]);
            AvailableCommand = available.Select(item => Util.NullableCaster.CastString(item)).ToList();

            var levelLayoutArray = Util.NullableCaster.CastJArray(token["LevelLayout"]);
            LevelLayout = new List<List<CellLayout>>();

            foreach (JArray row in levelLayoutArray)
            {
                var rowList = new List<CellLayout>();
                foreach (var cell in row)
                {
                    rowList.Add(new CellLayout(cell));
                }

                LevelLayout.Add(rowList);
            }
        }
        
        public Level(int number, List<string> availableCommands, PlayerDirection direction,
            CellLayout[,] levelLayoutArray, int bufferSizeCurrentValue, int p1SizeCurrentValue,
            int p2SizeCurrentValue, int startX, int startY)
        {
            number = number;
            AvailableCommand = availableCommands ?? new List<string>();
            direction = direction;
            maxBufferSize = bufferSizeCurrentValue;
            maxP1Size = p1SizeCurrentValue;
            maxP2Size = p2SizeCurrentValue;
            startX = startX;
            startY = startY;

            // Convert CellLayout[,] to List<List<CellLayout>>
            LevelLayout = new List<List<CellLayout>>();
            if (levelLayoutArray != null)
            {
                for (int i = 0; i < levelLayoutArray.GetLength(0); i++)
                {
                    var rowList = new List<CellLayout>();
                    for (int j = 0; j < levelLayoutArray.GetLength(1); j++)
                    {
                        rowList.Add(levelLayoutArray[i, j]);
                    }

                    LevelLayout.Add(rowList);
                }
            }
        }
    }
}