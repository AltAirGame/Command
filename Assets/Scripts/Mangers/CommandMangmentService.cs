using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


namespace GameSystems.Core
{
    public class CommandMangmentService : MonoBehaviour, ICommandMangmentService
    {
        public static event Action<Action, string> UpdatePlay;
        public static event Action ChangePlayButtonInteractivityStatus;
        public static event Action ResetPlayerPosition;
        public static event Action<int, ICommand> AddToBuffer;
        public static event Action<int, int, int> RemoveAtIndexofBuffer;
        
    
        private ILevelManagmentService levelManager;
        private WaitForSeconds WaitForSeconds = new WaitForSeconds(.1f);
        
        private int curentBufferIndex = 0;
        public GameObject subjectOFCommand;
        public static CommandMangmentService current;

        //A Pointer To Correct Command Buffer

        public List<ICommand> MainCommand = new List<ICommand>();
        public List<ICommand> P1Command = new List<ICommand>();
        public List<ICommand> p2Command = new List<ICommand>();

        public void StopPlay()
        {
            ServiceLocator.Instance.StopAllCoroutines();
            ServiceLocator.Instance.GetService<ILevelManagmentService>().ResetLevel();
            UpdatePlay?.Invoke(() => { Play(); }, "Play");
        }

        private void Awake()
        {
            current = this;
            Util.ShowMessage($"Command Factory Initlized");
            CommandFactory.GetCommand($"");
            var commandA = new Move();
            var commandB = new Move();
        }

        private void Start()
        {
            levelManager = ServiceLocator.Instance.GetService<ILevelManagmentService>();
        }

        public void SetSubjectOfCommand(GameObject subjectRefrence)
        {
            subjectOFCommand = subjectRefrence;
            ClearAllBuffers();
            SetCurrentBuffer(0);

            UpdatePlay?.Invoke(() => { Play(); }, "Play");
        }

        private void ClearAllBuffers()
        {
            MainCommand.Clear();
            P1Command.Clear();
            p2Command.Clear();
        }

        public void Play()
        {
            StartCoroutine(PlayWithDelay());
        }

        private IEnumerator PlayWithDelay()
        {
            UpdatePlay?.Invoke(() => { StopPlay(); }, "Stop");
            Util.ShowMessage($" the Current MainCommand has {MainCommand.Count} item in it");
            for (int i = 0; i < MainCommand.Count; i++)
            {
                yield return ServiceLocator.Instance.RunCoroutine(MainCommand[i].Execute(subjectOFCommand));
                yield return WaitForSeconds;
            }
        }

        public void AddToCurrentBuffer(ICommand command)
        {
            if (curentBufferIndex == 0)
            {
                if (MainCommand.Count < levelManager.CurrentLevel.maxBufferSize)
                {
                    MainCommand.Add(command);
                    AddToBuffer?.Invoke(curentBufferIndex, command);
                }
            }
            else if (curentBufferIndex == 1)
            {
                if (P1Command.Count < levelManager.CurrentLevel.maxP1Size)
                {
                    P1Command.Add(command);
                    AddToBuffer?.Invoke(curentBufferIndex, command);
                }
            }
            else if (curentBufferIndex == 2)
            {
                if (p2Command.Count < levelManager.CurrentLevel.maxP2Size)
                {
                    p2Command.Add(command);
                    AddToBuffer?.Invoke(curentBufferIndex, command);
                }
            }
        }

        public void RemoveFromBuffer(int bufferIndex, ICommand command, int instanceId)
        {
            switch (bufferIndex)
            {
                case 0:
                {
                    if (MainCommand.Count == 0)
                        return;
                    var commandTORemove = MainCommand.Where(x => x == command).First();


                    var index = MainCommand.IndexOf(command);
                    RemoveAtIndexofBuffer?.Invoke(bufferIndex, index, instanceId);
                    MainCommand.Remove(command);


                    break;
                }
                case 1:
                {
                    if (P1Command.Count == 0)
                        return;
                    var index = P1Command.IndexOf(command);
                    RemoveAtIndexofBuffer?.Invoke(bufferIndex, index, instanceId);
                    P1Command.Remove(command);
                    break;
                }
                case 2:
                {
                    if (p2Command.Count == 0)
                        return;
                    var index = p2Command.IndexOf(command);
                    RemoveAtIndexofBuffer?.Invoke(bufferIndex, index, instanceId);
                    p2Command.Remove(command);
                    break;
                }
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
}