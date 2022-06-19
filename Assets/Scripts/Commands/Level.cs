using System.Collections.Generic;
using MHamidi;


[System.Serializable]
public class Level
{
    public string name;
    public List<ICommand> AvailableCommand=new List<ICommand>();
    public ICommand[] LevelBufferSize;
    public ICommand[] P1BufferSize;
    public ICommand[] P2BufferSize;
    public int[,] LevelLayout;

    public Level(List<ICommand> availableCommand,int [,] levelLayout, int bufferSize,int p1BufferSize,int p2BufferSize)
    {
        
        AvailableCommand = availableCommand;
        LevelBufferSize = new ICommand[bufferSize];

    }
    public Level(string name,List<int> availableCommand,int [,] levelLayout, int bufferSize,int p1BufferSize,int p2BufferSize)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            this.name = $"unknown Level";
        }
        else
        {
            this.name = name;
        }

        this.LevelLayout = levelLayout;
        AvailableCommand=new List<ICommand>();
        for (int i = 0; i < availableCommand.Count; i++)
        {
            switch (availableCommand[i])
            {
                case 0:
                    AvailableCommand.Add(new MoveCommand());
                    break;
                case 1:
                    AvailableCommand.Add(new JumpCommand());
                    break;
                case 2:
                    AvailableCommand.Add(new TurnRightCommand());
                    break;
                case 3:
                    AvailableCommand.Add(new TurnLeftCommand());
                    break;
                case 4:
                    AvailableCommand.Add(new InteractCommand());
                    break;
                case 5:
                    AvailableCommand.Add(new FirstBufferCommand());
                    break;
                    
                case 6:
                    AvailableCommand.Add(new SecondBufferCommand());
               break;
            }
        }
        LevelBufferSize = new ICommand[bufferSize];
        P1BufferSize = new ICommand[p1BufferSize];
        P2BufferSize = new ICommand[p2BufferSize];

    }

    public Level(List<int> availableCommand, ICellEditor[,] levelLayout, int mainBufferSize, int p1BufferSize, int p2BufferSize)
    {
       
    }
}