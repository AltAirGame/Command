using System;
using System.Collections.Generic;
using MHamidi.UI.UI_Messages;
using UnityEngine;
using Utils.Singlton;


namespace MHamidi
{
    public interface IGameManger
    {
        void NextLevel();
        void StartLevelZero();
        void StartLevel(Level level);
        Level GetNextLevel();
        void UpdateLevelBufferUi(int bufferSize, int p1Size, int p2Size);
        void UpdatePlayerInputUI(List<string> avilableCommand);
    }

    public class GameManger : MonoBehaviour, IGameManger
    {
        // Start A Level
        // Start Next Level 
        //....


        public static event Action<List<string>> UpdatePlayerInput;
        public static event Action<string> UpdateLevelNameText;
        public static event Action<int, int, int> UpdateBufferUi;
      

        private void OnEnable()
        {
            LevelManger3D.CurrentLevelEnded += NextLevel;
        }

        private void OnDisable()
        {
            LevelManger3D.CurrentLevelEnded -= NextLevel;
        }

        public void NextLevel()
        {    Dipendency.Instance.DataManger.playerData.unlockedLevels.Add(CurrentLevel);
            Util.ShowMessag($" Next Level");
            Dipendency.Instance.UiManager.ShowMessage(new ModalWindowData("Congragulation", "You Finished This Level","nextLevel","Close",
                new SlidInOut(), () =>
                {
                    var nextLevel=GetNextLevel();
                    if (nextLevel is null)
                    {   //TODO Add ACtion to this Buttons
                        Dipendency.Instance.UiManager.ShowMessage(new ModalWindowData(" Game Ended "," you Finished the Game ","Start over","Quit the Game",new SlidInOut(),
                            () => { StartLevelZero();},
                            () =>
                            {
                                Dipendency.Instance.InputManager.OnQuit();
                            }));
                    }
                    else
                    {
                        StartLevel(nextLevel);
                        Util.ShowMessag($" We Are Loading Level {nextLevel.number} that is {Dipendency.Instance.DataManger.gameData.levels.IndexOf(nextLevel)}");
                    }
                    
                }));
        }


        public bool played = false;
        private IData dataManger;
        private ILevelManger _levelManger;
        public List<Level> levels;
        private int CurrentLevel=0;
        [SerializeField] private RectTransform mechanicParrent;
        public GameButton ButtonPrefab;

        private void Start()
        {
            _levelManger = Dipendency.Instance.LevelManger;
            dataManger = Dipendency.Instance.DataManger;
           

        }


        public void StartLevelZero()
        {
            CurrentLevel = 0;
            if (dataManger is not null)
            {
                StartLevel(dataManger.gameData.GetLevel(0));
            }

            else
            {
                Util.ShowMessag($"DataManger is null");
            }
        }

        public void StartLevel(Level level)
        {
            CurrentLevel = level.number;
            UpdateLevelText();
            if (_levelManger is not null)
            {
                _levelManger.CreatLevel(level, Dipendency.Instance.ComandManger.SetSubjectOfCommand); //Start A Level 
                UpdatePlayerInputUI(level.AvailableCommand);
                UpdateLevelBufferUi(level.maxBufferSize, level.maxP1Size, level.maxP2Size);
            }
            else
            {
                Util.ShowMessag($"LevelManger is null");
            }
        }

        public Level GetNextLevel()
        {
           
            if (Dipendency.Instance.DataManger.gameData.levels.Count==CurrentLevel+1)
            {
                Util.ShowMessag($" No next level ");
                return null;
            }

            CurrentLevel++;
            return Dipendency.Instance.DataManger.gameData.GetLevel(CurrentLevel);
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
            UpdateLevelNameText?.Invoke((CurrentLevel+1).ToString());
        }
    }
}