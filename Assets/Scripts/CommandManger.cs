using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MHamidi;
using UnityEngine;
using System;
public class CommandManger : MonoBehaviour
{
    
    
    public static event Action<Action, string> UpdatePlay;
    public static event Action ChangePlayButtonInteractivityStatus;
    public static event Action ResetPlayerPosition;
    
    private int order;
    public GameObject subjectOFCommand;
    public static CommandManger current;
    
    //A Pointer To Correc Command Buffer
    public List<ICommand> CurrentCommandBuffer;
    public List<ICommand> MainCommand=new List<ICommand>();
    public List<ICommand> P1Command=new List<ICommand>();
    public List<ICommand> p2Command =new List<ICommand>();
    public Dictionary<int, ICommand> commandLookUpTable=new Dictionary<int, ICommand>();
    
    
    
    
    private void Awake()
    {
        current = this;
        ConfigureLookups();
    }

    public void SetSubjectOfCommand(GameObject subjectRefrence)
    {
        subjectOFCommand = subjectRefrence;
        ClearAllBuffers();
        SetCurrentBuffer(MainCommand);
        
        UpdatePlay?.Invoke(() => { Play();},"PlayGame");
    }

    private void ClearAllBuffers()
    {
        MainCommand.Clear();
        P1Command.Clear();
        p2Command.Clear();
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
       
        ChangePlayButtonInteractivityStatus?.Invoke();
        UpdatePlay?.Invoke(() => { Rewind();},"Rewind");
        StartCoroutine(PlayWithDelay());
        
    }

    private IEnumerator PlayWithDelay()
    {
     
        foreach (var item in CurrentCommandBuffer)
        {
            item.Execute(subjectOFCommand);
            yield return new WaitForSeconds(.4f);
        }
        ChangePlayButtonInteractivityStatus?.Invoke(); 
    }

    public void Rewind()
    {
        ChangePlayButtonInteractivityStatus?.Invoke();
        UpdatePlay?.Invoke(() => { Play();},"Play");
        StartCoroutine(RewindWithDelay());
       
    }

    private IEnumerator RewindWithDelay()
    {
        
        foreach (var item in Enumerable.Reverse(CurrentCommandBuffer))
        {
            item.Undo(subjectOFCommand);
            yield return new WaitForSeconds(.4f);
        }
        ChangePlayButtonInteractivityStatus?.Invoke(); 
       
    }


    public void RestCurrentCommand()
    {
        CurrentCommandBuffer.Clear();
    }
    
    public void AddToCurrentBuffer(ICommand command)
    {
        
        CurrentCommandBuffer.Add(command);

        
        
    }
    public void RemoveFromBuffer(int index)
    {
        CurrentCommandBuffer.RemoveAt(index);
    
    }


    public void SetCurrentBuffer(List<ICommand> Bufer)
    {
        CurrentCommandBuffer = Bufer;
    }
}