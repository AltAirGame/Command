using System.Collections;
using UnityEngine;


namespace GameSystems.Core
{
    public class P1 : ICommand
    {
        public string Name
        {
            get => GetType().Name.ToLower();
        }

        public P1()
        {
        }

        public GameObject SubjectOfCommands { get; set; }


        public IEnumerator Execute(GameObject subject)
        {
            var buffer = CommandMangmentService.current.P1Command;
            foreach (var item in buffer)
            {
                yield return ServiceLocator.Instance.RunCoroutine(item.Execute(subject));
                yield return Util.GetWaitForSeconds(.2f);
            }
        }

        public IEnumerator Undo(GameObject subject)
        {
            yield return null;
        }

        public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward,
            int playerHeight,
            int forwardHeight)
        {
            return true;
        }

        public void ExecutionInstruction(ILevelManagmentService levelManagmentService)
        {
        }
    }
}