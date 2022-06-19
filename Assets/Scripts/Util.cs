using System;
using UnityEngine;

namespace MHamidi
{
    public enum TextColor
    {
        Red,Green,Blue,White,Yellow,Black
    }

    public static class Util
    {
        public static bool IsLoging = true;
        static Util()
        {
            
        }

        public static void ShowMessag (string message, TextColor color=TextColor.White)
        {
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
            
            
        }

    }
}