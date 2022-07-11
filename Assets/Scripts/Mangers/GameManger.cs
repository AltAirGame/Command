using System;
using System.Collections.Generic;
using MHamidi.UI.UI_Messages;
using UnityEngine;
using Utils.Singlton;


namespace MHamidi
{
    public class GameManger : MonoBehaviour
    {
        // Start A Level
        // Start Next Level 
        //....


        public static event Action<List<string>> UpdatePlayerInput;
        public static event Action<int, int, int> UpdateBufferUi;


        private void OnEnable()
        {
            LevelManger3D.CurrentLevelEnded += NextLevel;
        }

        private void OnDisable()
        {
            LevelManger3D.CurrentLevelEnded -= NextLevel;
        }

        private void NextLevel()
        {
            Util.ShowMessag($" Next Level");
            Dipendency.Instance.UiManager.ShowMessage(new ModalWindowData("Congragulation", "You Finished This Level",
                new SlidInOut(), () =>
                {
                    var nextLevel=GetNextLevel();
                    if (nextLevel is null)
                    {
                            
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

        private void StartLevel(Level level)
        {
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

        private Level GetNextLevel()
        {
            if (Dipendency.Instance.DataManger.gameData.levels.Count==CurrentLevel+1)
            {
                Util.ShowMessag($" No next level ");
                return null;
            }
            return Dipendency.Instance.DataManger.gameData.GetLevel(CurrentLevel + 1);
        }

        private void UpdateLevelBufferUi(int bufferSize, int p1Size, int p2Size)
        {
            UpdateBufferUi?.Invoke(bufferSize, p1Size, p2Size);
        }

        private void UpdatePlayerInputUI(List<string> avilableCommand)
        {
            UpdatePlayerInput?.Invoke(avilableCommand);
        }
    }
}