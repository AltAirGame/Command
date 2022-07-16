using System.Collections;
using DG.Tweening;
using UnityEngine;
using Utils.Singlton;

namespace MHamidi
{
    
    public class Right : ICommand
    {
        public string Name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public GameObject SubjectOfCommands { get; set; }

        public Right()
        {
           
        }

  
  
        public IEnumerator Execute(GameObject subject)
        {
            yield return Util.GetWaitForSeconds(.2f);
            TurnRight(subject);
            
        }

        public IEnumerator Undo(GameObject subject)
        {
            TurnLeft(subject);
            yield return null;
        }

        public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward, int playerHeight,
            int forwardHeight)
        {
            return true;
        }

        public void ExecutionInstruction(ILevelManger levelManger)
        {
            
        }

        public bool ExecutionInstruction(GameObject subjectOfCommand, Vector3Int playerPosition, Vector3Int playerForward,
            int playerHeight, int forwardHeight)
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