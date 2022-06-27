using System.Collections.Generic;
using System.Linq;
using MHamidi;
using UnityEngine;

public class CommandManger : MonoBehaviour
{
    private int order;
    public static CommandManger current;
    public List<ICommand> CommandBuffer = new List<ICommand>();
    public Dictionary<int, ICommand> commandLookUpTable=new Dictionary<int, ICommand>();

    
    
    
    private void Awake()
    {
        current = this;
        ConfigureLookups();
    }

    
    //Can Be Loaded From a json File
    public void ConfigureLookups()
    {
        
        commandLookUpTable.Add(0,new MoveCommand());
        commandLookUpTable.Add(1,new JumpCommand());
        commandLookUpTable.Add(2,new TurnRightCommand());
        commandLookUpTable.Add(3,new TurnLeftCommand());
        commandLookUpTable.Add(4,new InteractCommand());
        commandLookUpTable.Add(5,new FirstBufferCommand());
        commandLookUpTable.Add(6,new SecondBufferCommand());
        
        
    }

    public void Play()
    {
        foreach (var item in CommandBuffer)
        {
            item.Execute();
        }
    }

    public void Rewind()
    {
        foreach (var item in Enumerable.Reverse(CommandBuffer))
        {
            item.Undo();
        }  
    }

    public void Rest()
    {
        CommandBuffer.Clear();
    }
    
    public void AddToBuffer(ICommand command)
    {
        CommandBuffer.Add(command);
        Util.ShowMessag(command.name,TextColor.White);
    }
    public void RemoveFromBuffer(int index)
    {
        CommandBuffer.RemoveAt(index);
    
    }
    

}