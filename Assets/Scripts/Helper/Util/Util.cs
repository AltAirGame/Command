#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace GameSystems.Core
{
    public enum TextColor
    {
        Red,
        Green,
        Blue,
        White,
        Yellow,
        Black
    }

    public enum Direction
    {
        Forward,
        Back,
        Right,
        Left
    }

    public static class Util
    {
        public static bool IsLoging = true;
        public static event Action<string> Log;
        public static Dictionary<float, WaitForSeconds> WaitForSecondsMap=new Dictionary<float, WaitForSeconds>();
        static Util()
        {
        }

        public static WaitForSeconds GetWaitForSeconds(float time)
        {
            if (WaitForSecondsMap.ContainsKey(time))
            {
                return WaitForSecondsMap[time];
            }
            else
            {
                var wait=new WaitForSeconds(time);
                WaitForSecondsMap.Add(time,wait);
                return wait;

            }
           
        }

        public static class NullableCaster
        {
            public static int CastInt(JToken? data)
            {
                if (data != null && int.TryParse(data.ToString(), out int result))
                {
                    return result;
                }
                return 0;
            }

            public static bool CastBool(JToken? data)
            {
                if (data != null && bool.TryParse(data.ToString(), out bool result))
                {
                    return result;
                }
                return false;
            }

            public static string CastString(JToken? data)
            {
                return data?.ToString() ?? string.Empty;
            }

            public static JArray CastJArray(JToken? data)
            {
                return data as JArray ?? new JArray();
            }
        }

        public static class LevelLoader
        {
            public static LevelData LoadLevels()
            {
                // Load JSON text from a file in Resources folder
                var jsonText = Resources.Load<TextAsset>("Save/Game").text;
            
                // Parse JSON text into a JObject
                var jsonObject = JObject.Parse(jsonText);
            
                // Create a new LevelData instance
                var levelData = ScriptableObject.CreateInstance<LevelData>();
                levelData.levels = jsonObject["levels"]
                    .Select(token => new Level(token))
                    .ToList();

                return levelData;
            }
        
    }


        public static Byte[] SerilizeStringToByte(string file)
        {
            return Encoding.UTF8.GetBytes(file);
        }

        public static string GetPersistentDataPath(string SaveFile)
        {
            return Path.Combine(Application.persistentDataPath, SaveFile);
        }

        public static Direction ObjectForwardToWorld(Vector3 forward)
        {
            var forwardValue = Vector3.Dot(Vector3.forward, forward);
            var RightValue = Vector3.Dot(Vector3.forward, forward);
            if (forwardValue != 0)
            {
                if (forwardValue == 1)
                {
                    return Direction.Forward;
                }
                else
                {
                    return Direction.Back;
                }
            }
            else if (RightValue != 0)
            {
                if (RightValue == 1)
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Left;
                }
            }

            return Direction.Forward;
        }

        public static void ShowMessage(string message, TextColor color = TextColor.White)
        {
#if UNITY_EDITOR


            if (IsLoging is false)
            {
                return;
            }

            switch (color)
            {
                case TextColor.Red:
                    Debug.Log($"<color=red> {message} </color>");
                    break;
                case TextColor.Green:
                    Debug.Log($"<color=green> {message} </color>");
                    break;
                case TextColor.Blue:
                    Debug.Log($"<color=blue> {message} </color>");
                    break;
                case TextColor.White:
                    Debug.Log($"<color=white> {message} </color>");
                    break;
                case TextColor.Yellow:
                    Debug.Log($"<color=yellow> {message} </color>");
                    break;
                case TextColor.Black:
                    Debug.Log($"<color=black> {message} </color>");
                    break;
                default:
                    break;
            }
#endif
            Log?.Invoke(message);
        }

        public static string GetResureceDataPath(string saveFile)
        {
            return Path.Combine("Resources/Save/" + saveFile);
        }
    }
}