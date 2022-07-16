using System.Collections;
using UnityEngine;
using Utils.Singlton;

namespace MHamidi
{
    public class Left : ICommand
    {
        public string Name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
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

        public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward, int playerHeight,
            int forwardHeight)
        {
            return true;
        }
        private void TurnRight(GameObject subject)
        {
            
            Dipendency.Instance.GameEventHandler.OnRotate(true);
        }

        private void TurnLeft(GameObject subject)
        {  Dipendency.Instance.GameEventHandler.OnRotate(false);
        }
    }
}