using System;
using System.Collections.Generic;
using UnityEngine;


namespace GameSystems.Core.Game
{
    public class GameManger : MonoBehaviour, IGameManger
    {
        // Start A Level
        // Start Next Level 
        //....

        public bool played = false;
        private IDataManagementService dataManagmentServiceManger;
        private ILevelManagmentService levelManagmentService;
        private ICommandMangmentService commandMangmentService;
        private IInputManagementService inputManagmentService;
        private IUiManager uiManger;
        public List<Level> levels;
        private int CurrentLevel = 0;
        [SerializeField] private RectTransform mechanicParrent;
        public GameButton ButtonPrefab;



        private void Start()
        {
            levelManagmentService = ServiceLocator.Instance.GetService<ILevelManagmentService>();
            dataManagmentServiceManger = ServiceLocator.Instance.GetService<IDataManagementService>();
            commandMangmentService = ServiceLocator.Instance.GetService<ICommandMangmentService>();
            uiManger = ServiceLocator.Instance.GetService<IUiManager>();
            inputManagmentService = ServiceLocator.Instance.GetService<IInputManagementService>();
        }


        private void OnEnable()
        {
            LevelManagementService3D.CurrentLevelEnded += NextLevel;
        }

        private void OnDisable()
        {
            LevelManagementService3D.CurrentLevelEnded -= NextLevel;
        }

        public event Action<List<string>> UpdatePlayerInput;
        public event Action<string> UpdateLevelNameText;
        public event Action<int, int, int> UpdateBufferUi;

        public void NextLevel()
        {
            dataManagmentServiceManger.PlayerData.unlockedLevels.Add(CurrentLevel);
            Util.ShowMessage($" Next Level");
            uiManger.ShowMessage(new ModalWindowData("Congragulation", "You Finished This Level", "nextLevel", "Close",
                new SlidInOut(), () =>
                {
                    var nextLevel = GetNextLevel();
                    if (nextLevel is null)
                    {
                        //TODO Add ACtion to this Buttons
                        uiManger.ShowMessage(new ModalWindowData(" Game Ended ", " you Finished the Game ",
                            "Start over", "Quit the Game", new SlidInOut(),
                            () => { StartLevelZero(); },
                            () => { inputManagmentService.OnQuit(); }));
                    }
                    else
                    {
                        StartLevel(nextLevel);
                        Util.ShowMessage(
                            $" We Are Loading Level {nextLevel.number} that is {dataManagmentServiceManger.LevelData.levels.IndexOf(nextLevel)}");
                    }
                }));
        }


        public void StartLevelZero()
        {
            CurrentLevel = 0;
            if (dataManagmentServiceManger is not null)
            {
                StartLevel(dataManagmentServiceManger.LevelData.GetLevel(0));
            }

            else
            {
                Util.ShowMessage($"DataManger is null");
            }
        }

        public void StartLevel(Level level)
        {
            CurrentLevel = level.number;
            UpdateLevelText();
            if (levelManagmentService is not null)
            {
                levelManagmentService.CreatLevel(level, commandMangmentService.SetSubjectOfCommand); //Start A Level 
                UpdatePlayerInputUI(level.AvailableCommand);
                UpdateLevelBufferUi(level.maxBufferSize, level.maxP1Size, level.maxP2Size);
            }
            else
            {
                Util.ShowMessage($"LevelManger is null");
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public Level GetNextLevel()
        {
            if (dataManagmentServiceManger.LevelData.levels.Count == CurrentLevel + 1)
            {
                Util.ShowMessage($" No next level ");
                return null;
            }

            CurrentLevel++;
            return dataManagmentServiceManger.LevelData.GetLevel(CurrentLevel);
        }

        public void UpdateLevelBufferUi(int bufferSize, int p1Size, int p2Size)
        {
            UpdateBufferUi?.Invoke(bufferSize, p1Size, p2Size);
        }

        public void UpdatePlayerInputUI(List<string> avilableCommand)
        {
            UpdatePlayerInput?.Invoke(avilableCommand);
        }

        private void UpdateLevelText()
        {
            UpdateLevelNameText?.Invoke((CurrentLevel + 1).ToString());
        }
    }
}