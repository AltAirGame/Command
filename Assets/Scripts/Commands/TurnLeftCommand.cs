using UnityEngine;

namespace MHamidi
{
    public class TurnLeftCommand : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public bool executeWasSuccessful { get; set; }

        public void Execute(GameObject subject)
        {
            TurnLeft(subject);
        }

        public void Undo(GameObject subject)
        {
            TurnRight(subject);
        }

     
        private void TurnRight(GameObject subject)
        {
          subject.transform.Rotate(Vector3.up,90);
            
        }

        private void TurnLeft(GameObject subject)
        {  subject.transform.Rotate(Vector3.up,-90);
        }
    }
}