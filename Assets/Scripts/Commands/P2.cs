using System.Collections;
using UnityEngine;

namespace MHamidi
{
    public class P2 : ICommand
    {
       
        public string Name {   get { return this.GetType().Name.ToLower(); }
            set { }}

        public P2()
        {
           
        }

        public GameObject SubjectOfCommands { get; set; }
        

        public IEnumerator Execute(GameObject subject)
        {
            throw new System.NotImplementedException();
            yield return null;
        }

        public IEnumerator Undo(GameObject subject)
        {
            throw new System.NotImplementedException();
            yield return null;
        }

        public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward, int playerHeight,
            int forwardHeight)
        {
            return true;
        }

      
      
    }
}