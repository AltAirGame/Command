using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Core
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public List<Level> levels;

        public Level GetLevel(int value)
        {
            return levels[value];
        }
    }
}