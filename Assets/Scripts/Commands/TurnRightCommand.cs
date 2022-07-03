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

        public void Execute(GameObject subject)
        {
            TurnRight(subject);
        }

        public void Undo(GameObject subject)
        {
            TurnLeft(subject);
        }

        

        private void TurnRight(GameObject subject)
        {
            subject.transform.Rotate(Vector3.up,90);
            
        }

        private void TurnLeft(GameObject subject)
        {  subject.transform.Rotate(Vector3.up,-90f);
        }
    }
}