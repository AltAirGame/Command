using System;
using UnityEngine;

namespace GameSystems.Core
{

    public interface ILevelManagmentService
    {
        public GameObject Player { get; set; }
        public Level CurrentLevel { get; set; }
        public Vector3Int PlayerPos { get; set; }
        public Vector3Int PlayerForward { get; set; }
        void Intereact();
        public Vector3Int GetFrontOfPlayerPosition();

        public int GetFrontOfPlayerHeight();
        public int GetPlayerCurrentHeight();

        public void CreatLevel(Level level, Action<GameObject> setSubjectOfCommand);
        public void ResetLevel();
        public bool CheckIfGameEnded();
        bool IsAvailable(ICommand command);
        public void Rotate(bool isRight);
        public void UpdatePlayer();
    }
}