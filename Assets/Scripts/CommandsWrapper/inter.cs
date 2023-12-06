using System.Collections;
using UnityEngine;


namespace GameSystems.Core
{
    public class Inter : ICommand
    {
        public string Name
        {
            get => GetType().Name.ToLower();
        }

        public GameObject SubjectOfCommands { get; set; }

        public bool Done { get; set; }

        public bool executeWasSuccessful { get; set; }

        public IEnumerator Execute(GameObject subject)
        {
            SubjectOfCommands = subject;
            Interact();
            subject.GetComponentInChildren<IPlayerAnimation>().InterAct();
            yield return null;
        }

        public IEnumerator Undo(GameObject subject)
        {
            SubjectOfCommands = subject;
            ReverceInteract();
            subject.GetComponentInChildren<IPlayerAnimation>().InterAct();
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


        public void Interact()
        {
            Interacting();
        }

        public void ReverceInteract()
        {
            Interacting();
        }

        public void Interacting()
        {
            ServiceLocator.Instance.GetService<ILevelManagmentService>().Intereact();
        }
    }
}