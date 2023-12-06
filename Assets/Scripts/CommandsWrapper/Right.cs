using System.Collections;
using UnityEngine;


namespace GameSystems.Core
{
    public class Right : ICommand
    {
        public string Name
        {
            get => GetType().Name.ToLower();
        }

        public GameObject SubjectOfCommands { get; set; }

        public Right()
        {
        }


        public IEnumerator Execute(GameObject subject)
        {
            yield return Util.GetWaitForSeconds(.2f);
            TurnRight(subject);
            yield return Util.GetWaitForSeconds(.1f);
        }

        public IEnumerator Undo(GameObject subject)
        {
            TurnLeft(subject);
            yield return null;
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