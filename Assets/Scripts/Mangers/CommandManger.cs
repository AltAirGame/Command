using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MHamidi;
using UnityEngine;
using System;
using Utils.Singlton;
using Object = System.Object;

public class CommandManger : MonoBehaviour
{
    
    
    public static event Action<Action, string> UpdatePlay;
    public static event Action ChangePlayButtonInteractivityStatus;
    public static event Action ResetPlayerPosition;
    public static event Action<int,ICommand> AddToBuffer;
    public static event Action<int,int> RemoveAtIndexofBuffer;


    private int curentBufferIndex=0;
    public GameObject subjectOFCommand;
    public static CommandManger current;
    
    //A Pointer To Correc Command Buffer
   
    public List<ICommand> MainCommand=new List<ICommand>();
    public List<ICommand> P1Command=new List<ICommand>();
    public List<ICommand> p2Command =new List<ICommand>();
    public Stack<ICommand> RewindStack=new Stack<ICommand>();

    public List<ICommand> RunTimeCommands = new List<ICommand>(); 
    public Dictionary<int, ICommand> commandLookUpTable=new Dictionary<int, ICommand>();
    
    
    
    
    private void Awake()
    {
        current = this;
        ConfigureLookups();

        var commandA = new MoveCommand();
        var commandB = new MoveCommand();
   
    }

    public void SetSubjectOfCommand(GameObject subjectRefrence)
    {
        subjectOFCommand = subjectRefrence;
        ClearAllBuffers();
        SetCurrentBuffer(0);
        
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
        Util.ShowMessag($" the Current MainCommand has {MainCommand.Count} item in it");
        foreach (var item in MainCommand)
        {
            yield return StartCoroutine(item.Execute(subjectOFCommand));
            RewindStack.Push(item);
            
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
        Util.ShowMessag($" [CommaandManger] Command Count is  {RewindStack.Count}");
        while (RewindStack.Count>0)
        {
            var Command=RewindStack.Pop();
            Util.ShowMessag($"{Command.GetHashCode()}",TextColor.Red);
            Util.ShowMessag($" [CommaandManger] Command Execution was {Command.executeWasSuccessful}");
            yield return StartCoroutine(Command.Undo(subjectOFCommand));
           
        }
        ChangePlayButtonInteractivityStatus?.Invoke(); 
       
    }


   
    
    public void AddToCurrentBuffer(ICommand command)
    {
        
        
        if (curentBufferIndex==0)
        {
            if (MainCommand.Count<Dipendency.Instance.LevelManger.currentLevel.maxBufferSize)
            {
                MainCommand.Add(command);
                AddToBuffer?.Invoke(curentBufferIndex,command);
        
            }
        }
        else if (curentBufferIndex == 1)
        {
            if (P1Command.Count<Dipendency.Instance.LevelManger.currentLevel.maxP1Size)
            {
                P1Command.Add(command);
                AddToBuffer?.Invoke(curentBufferIndex,command);
        
            }
        }
        else if (curentBufferIndex == 2)
        {
            if (p2Command.Count<Dipendency.Instance.LevelManger.currentLevel.maxP2Size)
            {
                p2Command.Add(command);
                AddToBuffer?.Invoke(curentBufferIndex,command);
        
            }
        }


    }
    public void RemoveFromBuffer(int bufferIndex ,int index)
    {
        if (bufferIndex==0)
        {
            MainCommand.RemoveAt(index);
            RemoveAtIndexofBuffer?.Invoke(curentBufferIndex,index); 
        }

        if (bufferIndex==1)
        {
            P1Command.RemoveAt(index);
            RemoveAtIndexofBuffer?.Invoke(curentBufferIndex,index); 
        }

        if (bufferIndex==2)
        {
            p2Command.RemoveAt(index);
            RemoveAtIndexofBuffer?.Invoke(curentBufferIndex,index); 
        }
      
        
    
    }

    private void ChangeBuffer(int index)
    { 
        curentBufferIndex = index;
       
    }

    public void SetCurrentBuffer(int index)
    {
       ChangeBuffer(index);
    }
}