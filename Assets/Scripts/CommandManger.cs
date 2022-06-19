using System.Collections.Generic;
using System.Linq;
using MHamidi;
using UnityEngine;

public class CommandManger : MonoBehaviour
{
    private int order;
    public static CommandManger current;
    public List<ICommand> CommandBuffer = new List<ICommand>();


    private void Awake()
    {
        current = this;
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