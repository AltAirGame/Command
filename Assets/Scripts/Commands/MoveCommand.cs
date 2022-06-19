using MHamidi;


public class MoveCommand : ICommand
{
    public MoveCommand()
    {
    }

    public string name
    {
        get { return this.GetType().Name.ToLower(); }
        set { }
    }

    public void Execute()
    {
        MoveForward();
    }

    public void Undo()
    {
        MoveBackWard();
    }

    private void MoveForward()
    {
        Util.ShowMessag($" [ MoveForward ] ", TextColor.Red);
    }

    private void MoveBackWard()
    {
        Util.ShowMessag($" [ MoveBack ] ", TextColor.Yellow);
    }
}