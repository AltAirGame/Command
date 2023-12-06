using System;
using System.Collections;
using UnityEngine;

namespace GameSystems.Core
{


    public interface ICommand
    {
        public string Name { get;  }
        public GameObject SubjectOfCommands { get; set; }
        public IEnumerator Execute(GameObject subject);
        public IEnumerator Undo(GameObject subject);

        public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward,
            int playerHeight,
            int forwardHeight);


    }
}