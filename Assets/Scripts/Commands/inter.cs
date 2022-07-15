using System.Collections;
using UnityEngine;
using Utils.Singlton;

namespace MHamidi
{
    public class inter : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
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
          

            Dipendency.Instance.LevelManger.Intereact();
        }
    }
}