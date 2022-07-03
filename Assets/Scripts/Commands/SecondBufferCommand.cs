using UnityEngine;

namespace MHamidi
{
    public class SecondBufferCommand : ICommand
    {
       
        public string name { get; set; }
        public bool executeWaseSuccesful { get; set; }

        public void Execute(GameObject subject)
        {
            throw new System.NotImplementedException();
        }

        public void Undo(GameObject subject)
        {
            throw new System.NotImplementedException();
        }

        public void Execute()
        {
           
        }

        public void Undo()
        {
            
        }
    }
}