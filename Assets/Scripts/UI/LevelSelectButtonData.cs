using System;
using UnityEngine;

namespace GameSystems.Core
{
    public class LevelSelectButtonData
    {
        public int Number;
        public bool isUnlocked;
        public Sprite icon;
        public Action onClick;

        public LevelSelectButtonData(int number, bool isUnlocked, Sprite icon, Action onClick)
        {
            Number = number;
            this.isUnlocked = isUnlocked;
            this.icon = icon;
            this.onClick = onClick;
        }
    }
}