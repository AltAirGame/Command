using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MHamidi;
using UnityEngine;
using System;
using Utils.Singlton;

public class CommandManger : MonoBehaviour
{
    
    
    public static event Action<Action, string> UpdatePlay;
    public static event Action ChangePlayButtonInteractivityStatus;
    public static event Action ResetPlayerPosition;
    public static event Action<int,ICommand> AddToBuffer;
    public static event Action<int,int> RemoveAtIdnexofBuffer;


    private int curentBufferIndex=0;
    public GameObject subjectOFCommand;
    public static CommandManger current;
    
    //A Pointer To Correc Command Buffer
   
    public List<ICommand> MainCommand=new List<ICommand>();
    public List<ICommand> P1Command=new List<ICommand>();
    public List<ICommand> p2Command =new List<ICommand>();
    public List<ICommand> RunTimeCommands = new List<ICommand>(); 
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
        
        commandLookUpTable.Add(0,new MoveCommand(subjectOFCommand));
        commandLookUpTable.Add(1,new JumpCommand(subjectOFCommand));
        commandLookUpTable.Add(2,new TurnRightCommand(subjectOFCommand));
        commandLookUpTable.Add(3,new TurnLeftCommand(subjectOFCommand));
        commandLookUpTable.Add(4,new InteractCommand(subjectOFCommand));
        commandLookUpTable.Add(5,new FirstBufferCommand(subjectOFCommand));
        commandLookUpTable.Add(6,new SecondBufferCommand(subjectOFCommand));
        
        
    }

    public void Play()
    {   
       
        ChangePlayButtonInteractivityStatus?.Invoke();
        UpdatePlay?.Invoke(() => { Rewind();},"Rewind");
        StartCoroutine(PlayWithDelay());
        
    }

    private IEnumerator PlayWithDelay()
    {
        //We Creat the RunTime Command
        CreatRunTimeCommand();
     
        foreach (var item in MainCommand)
        {
            yield return StartCoroutine(item.Execute(subjectOFCommand));
            
        }
        ChangePlayButtonInteractivityStatus?.Invoke(); 
    }

    private void CreatRunTimeCommand()
    {
        foreach (var item in MainCommand)
        {
            
          
            
            
        }
    }

    public void Rewind()
    {
        ChangePlayButtonInteractivityStatus?.Invoke();
        UpdatePlay?.Invoke(() => { Play();},"Play");
        StartCoroutine(RewindWithDelay());
       
    }

    private IEnumerator RewindWithDelay()
    {
        
        foreach (var item in Enumerable.Reverse(MainCommand))
        {
            item.Undo(subjectOFCommand);
            yield return new WaitForSeconds(.4f);
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
            RemoveAtIdnexofBuffer?.Invoke(curentBufferIndex,index); 
        }

        if (bufferIndex==1)
        {
            P1Command.RemoveAt(index);
            RemoveAtIdnexofBuffer?.Invoke(curentBufferIndex,index); 
        }

        if (bufferIndex==2)
        {
            p2Command.RemoveAt(index);
            RemoveAtIdnexofBuffer?.Invoke(curentBufferIndex,index); 
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