using UnityEngine;

namespace MHamidi
{
    public class JumpCommand : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public bool executeWaseSuccesful { get; set; }

        public void Execute(GameObject subject)
        {
            Util.ShowMessag($" Jump");
            Jump(subject);
            subject.GetComponentInChildren<IPlayerAnimation>().Jump();
            
        }

        public void Undo(GameObject subject)
        {
            Util.ShowMessag($"Jump Undo");
            RevercJump(subject);
            subject.GetComponentInChildren<IPlayerAnimation>().Jump();

        }

        private void Jump(GameObject subject)
        {
            LevelManger3D.Instance.Jump();
        }

        private void RevercJump(GameObject subject)
        {
            LevelManger3D.Instance.JumpBack();
        }
        
     
        
    }
}