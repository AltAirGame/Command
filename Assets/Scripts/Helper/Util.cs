#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MHamidi
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

        public static class NullabelCaster
        {
            public static JArray CastJArray(JToken? data)
            {
                if (data is null)
                {
                    return new JArray();
                }
                else
                {
                    if (data.HasValues)
                    {
                        if (data is JArray)
                        {
                            if (data.AsJEnumerable().Count() == 0)
                            {
                                return new JArray();
                            }
                            else
                            {
                                return (JArray)data;
                            }
                        }
                    }
                    else
                    {
                        return new JArray();
                    }
                }

                return new JArray();
            }


            public static int CastInt(JToken? data)
            {
                return (int?)data ?? 0;
            }

            public static bool CastBool(JToken? data)
            {
                return (bool?)data ?? false;
            }

            public static string CastString(JToken? data)
            {
                return (string?)data ?? string.Empty;
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

        public static void ShowMessag(string message, TextColor color = TextColor.White)
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