using System;
using System.IO;
using System.Text;
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

    public static class Util
    {
        public static bool IsLoging = true;

        static Util()
        {
        }

        public static class NullabelCaster
        {
            public static int CastInt(JToken? data)
            {
                return (int?)data ?? 0;
            }

            public static bool Castbool(JToken? data)
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
        }

    }
}