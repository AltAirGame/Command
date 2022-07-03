using System;
using System.Collections.Generic;
using UnityEngine;


namespace MHamidi
{
    public class GameManger : MonoBehaviour
    {
        
        // Start A Level
        // Start Next Level 
        //....



        public static event Action<List<int>> UpdatePlayerInput;
        public static event Action<int, int, int> UpdateBufferUi;

        



        public bool played=false;
        private DataManger dataManger;
        private ILevelManger _levelManger;
        public List<Level> levels;
        [SerializeField] private RectTransform mechanicParrent;
        public GameButton ButtonPrefab;

        private void Awake()
        {
            _levelManger = GetComponent<ILevelManger>();
            dataManger=DataManger.Instance;
        }

        private void Start()
        {

          
            
        }

        public void StartLevelZero()
        {
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
                _levelManger.CreatLevel(level,CommandManger.current.SetSubjectOfCommand); //Start A Level 
                UpdatePlayerInputUI(level.AvailableCommand);
                UpdateLevelBufferUi(level.maxBufferSize,level.maxP1Size,level.maxP2Size);
            }
            else
            {
                Util.ShowMessag($"LevelManger is null");
            }
           
            
         
            

        }

        private void UpdateLevelBufferUi(int bufferSize,int p1Size,int p2Size)
        {
            
            UpdateBufferUi?.Invoke(bufferSize,p1Size,p2Size);
        }

        private void UpdatePlayerInputUI(List<int> avilableCommand)
        {
            UpdatePlayerInput?.Invoke(avilableCommand);
          
        }
    }
}