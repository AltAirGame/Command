namespace MHamidi
{
    public class InteractCommand : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public void Execute()
        {
            Interact();
        }

        public void Undo()
        {
            ReverceInteract();
        }

        public void Interact()
        {
            Util.ShowMessag($" [ Interact ] ", TextColor.Red);
        }
        public void ReverceInteract()
        {
            Util.ShowMessag($" [ Interact ] ", TextColor.Yellow);
        }
    }
}