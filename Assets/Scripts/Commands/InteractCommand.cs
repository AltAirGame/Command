using System.Collections;
using UnityEngine;
using Utils.Singlton;

namespace MHamidi
{
    public class InteractCommand : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public InteractCommand(GameObject subjectOfCommands)
        {
            this.SubjectOfCommands = subjectOfCommands;
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
            Interacting();
            
        }
        public void ReverceInteract()
        {
           Interacting();
        }
        public void Interacting()
        {
            var playepos = SubjectOfCommands.transform.position;
            if (Dipendency.Instance.LevelManger.currentLevel.LevelLayout[(int)playepos.x, (int)playepos.z].Type == CellType.Interactable)
            {
                
            }

            Dipendency.Instance.LevelManger.CheckIfGameEnded();
        }

    }
}