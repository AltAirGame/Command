using System.Collections;
using UnityEngine;


namespace GameSystems.Core
{
    public class Left : ICommand
    {
        public string Name
        {
            get => GetType().Name.ToLower();
        }

        public Left()
        {
        }

        public GameObject SubjectOfCommands { get; set; }


        public IEnumerator Execute(GameObject subject)
        {
            yield return Util.GetWaitForSeconds(.2f);
            TurnLeft(subject);
            yield return Util.GetWaitForSeconds(.1f);
        }

        public IEnumerator Undo(GameObject subject)
        {
            TurnRight(subject);
            yield return Util.GetWaitForSeconds(.2f);
        }

        public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward,
            int playerHeight,
            int forwardHeight)
        {
            return true;
        }

        private void TurnRight(GameObject subject)
        {
            ServiceLocator.Instance.GetService<ILevelManagmentService>().Rotate(true);
        }

        private void TurnLeft(GameObject subject)
        {
            ServiceLocator.Instance.GetService<ILevelManagmentService>().Rotate(false);
        }
    }
}