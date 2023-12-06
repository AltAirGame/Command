using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameSystems.Core
{
   

    public class DataManagementServiceManager : MonoBehaviour,IDataManagementService
    {
        private const string PlayerDataFilename = "Player.txt";
        private const string GameDataFilename = "Game.txt";
        
        public PlayerData PlayerData { get; private set; }
        private LevelData _levelData;
        public LevelData LevelData
        {
            get
            {
                return _levelData;
            }
            
        }

        private IRepository repository;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            repository = ServiceLocator.Instance.GetService<IRepository>();
            if (repository is null)
            {
                Debug.LogError("repository is null ");
            }
            LoadPlayerData();
            LoadGameData();
        }

        public void AddLevel(Level level)
        {
            _levelData ??= new LevelData();
            LevelData.levels ??= new List<Level>();

            var existingLevel = LevelData.levels.FirstOrDefault(x => x.number == level.number);
            if (existingLevel != null)
            {
                var index = existingLevel.number;
                LevelData.levels.Add(level);
            }
            else
            {
                existingLevel = level;
            }

            SaveData();
        }

        public void SaveData()
        {
            Debug.Log($"{(repository is null?"repository is null ":"repository is not null")}");
            repository.SavePlayerData(PlayerDataFilename, PlayerData);
            repository.SaveGameData(GameDataFilename, LevelData);
        }

        public void EditPlayerData(PlayerData data)
        {
            PlayerData = data;
            SaveData();
        }

        public void LoadGameData()
        {
            Debug.Log($" <color=green>Loading Game Data </color> ");
            var response = repository.LoadGameData(GameDataFilename);
            if (string.IsNullOrWhiteSpace(response.error.message))
            {
                Debug.Log($"<color=yellow> There was no Error and there is {response.data.levels.Count} levels in GameData</color>");
                _levelData = response.data;
                return;
            }
            else
            {
                Debug.Log($"<color=red> Error Message Loading Game Data {response.error.message}</color>");
                _levelData = new LevelData();
                return;
            }

            Debug.Log($" <color=green>Loading Game Data </color> ");
            _levelData = string.IsNullOrWhiteSpace(response.error.message) ? response.data : new LevelData();
        }

        public void LoadPlayerData()
        {
            var response = repository.LoadPlayerData(PlayerDataFilename);
            PlayerData = string.IsNullOrWhiteSpace(response.error.message) ? response.data : new PlayerData();
        }

        private void OnDisable()
        {
           // SaveData();
        }
    }
}
