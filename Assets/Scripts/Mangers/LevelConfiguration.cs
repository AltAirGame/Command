using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameSystems.Core.Game
{
    [CreateAssetMenu(fileName = "LevelConfiguration", menuName = "GameSystems/LevelConfiguration", order = 1)]
    public class LevelConfiguration : ScriptableObject
    {
        public WaitForSeconds wait = new WaitForSeconds(.1f); 
        [Header("Level Dimensions")] public int levelWidth;
        public int levelHeight;
        
        [Header("Player Settings")] public Vector3 playerStartPosition;
        public float playerMoveSpeed;
        public GameObject playerPrefab;
        public GameObject Player { get; set; }


        [Header("Cell Settings")] public List<GameObject> Cells;
        public GameCell[,] gameCells;
        public List<GameCell> currentLevelInteractable;
        public float cellCreationDelay;

        [Header("Gameplay Parameters")] public int initialLives;
        public float timeLimit;

        [Header("Animation")] public Ease ease;


        public Level CurrentLevel { get; set; }
        public Vector3Int PlayerPos { get; set; }
        public Vector3Int PlayerForward { get; set; }

        

        public bool IsValid()
        {
            // Validate the configuration data
            // Example: return levelWidth > 0 && levelHeight > 0 && playerPrefab != null;
            return true;
        }
    }
}