namespace MHamidi
{
    public class JumpCommand : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public void Execute()
        {
            Jump();
        }

        public void Undo()
        {
            RevercJump();
        }

        private void Jump()
        {
            Util.ShowMessag($" [ Jump ] ", TextColor.Red);
        }   private void RevercJump()
        {
            Util.ShowMessag($" [ Jump ] ", TextColor.Yellow);
        }
    }
}