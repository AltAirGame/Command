namespace MHamidi
{
    public class TurnRightCommand : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public void Execute()
        {
            TurnRight();
        }

        public void Undo()
        {
            TurnLeft();
        }

        private void TurnRight()
        {
            Util.ShowMessag($" [ TurnRight ] ", TextColor.Red);
        }

        private void TurnLeft()
        {
            Util.ShowMessag($" [ TurnLeft ] ", TextColor.Yellow);
        }
    }
}