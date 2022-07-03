using UnityEngine;

namespace MHamidi
{
    public class InteractCommand : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public bool executeWasSuccessful { get; set; }

        public void Execute(GameObject subject)
        {
          Interact();
          subject.GetComponentInChildren<IPlayerAnimation>().InterAct();
        }

        public void Undo(GameObject subject)
        {
            ReverceInteract();
            subject.GetComponentInChildren<IPlayerAnimation>().InterAct();
        }

        public void Execute()
        {
            Interact();
            
        }

        public void Undo()
        {
            ReverceInteract();
        }

        public void Interact()
        {
            LevelManger3D.Instance.Interact();
            
        }
        public void ReverceInteract()
        {
            LevelManger3D.Instance.Interact();
        }
    }
}