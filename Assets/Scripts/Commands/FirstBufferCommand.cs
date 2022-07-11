using System.Collections;
using UnityEngine;

namespace MHamidi
{
    public class FirstBufferCommand : ICommand
    {
        public string name {    get { return this.GetType().Name.ToLower(); }
            set { } }

        public FirstBufferCommand()
        {
          
        }

        public GameObject SubjectOfCommands { get; set; }

        public bool Done { get; set; }
        public bool executeWasSuccessful { get; set; }

        public IEnumerator Execute(GameObject subject)
        {
            var buffer=CommandManger.current.P1Command;
            yield return null;
        }

        public IEnumerator Undo(GameObject subject)
        {
            yield return null;
        }

      
        
    }
}