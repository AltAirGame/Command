namespace MHamidi
{
    public class TurnLeftCommand : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public void Execute()
        {
            TurnLeft();
        }

        public void Undo()
        {
            TurnRight();
        }

        private void TurnRight()
        {
            Util.ShowMessag($" [ Turn Right ] ", TextColor.Yellow);
        }

        private void TurnLeft()
        {   Util.ShowMessag($" [ Turn Left ] ", TextColor.Red);
        }
    }
}