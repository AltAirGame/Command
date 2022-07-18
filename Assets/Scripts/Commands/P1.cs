using System.Collections;
using UnityEngine;
using Utils.Singlton;

namespace MHamidi
{
    public class P1 : ICommand
    {
        public string Name {    get { return this.GetType().Name.ToLower(); }
            set { } }

        public P1()
        {
          
        }

        public GameObject SubjectOfCommands { get; set; }

   

        public IEnumerator Execute(GameObject subject)
        {
            var buffer=CommandManger.current.P1Command;
            foreach (var item in buffer)
            {
                yield return Dipendency.Instance.StartCoroutine(item.Execute(subject));
                yield return Util.GetWaitForSeconds(.2f);
            }
            
        }

        public IEnumerator Undo(GameObject subject)
        {
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
    }
}