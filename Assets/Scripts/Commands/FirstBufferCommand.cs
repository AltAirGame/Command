using UnityEngine;

namespace MHamidi
{
    public class FirstBufferCommand : ICommand
    {
        public string name { get; set; }
        public bool executeWaseSuccesful { get; set; }

        public void Execute(GameObject subject)
        {
           
        }

        public void Undo(GameObject subject)
        {
           
        }

      
        
    }
}