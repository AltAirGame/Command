using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace MHamidi
{
    
    public class TurnRightCommand : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public GameObject SubjectOfCommands { get; set; }

        public TurnRightCommand(GameObject subjectOfCommands)
        {
            this.SubjectOfCommands = subjectOfCommands;
        }

        public bool Done { get; set; }
        public bool executeWasSuccessful { get; set; }
        public IEnumerator Execute(GameObject subject)
        {
            TurnRight(subject);
            yield return null;
        }

        public IEnumerator Undo(GameObject subject)
        {
            TurnLeft(subject);
            yield return null;
        }
        private void TurnRight(GameObject subject)
        {
            subject.transform.Rotate(new Vector3(0f,90f,0f));
        }

        private void TurnLeft(GameObject subject)
        {  subject.transform.Rotate(new Vector3(0f,-90f,0f));
        }
    }
}