using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace MHamidi
{


    public interface IData
    {
        public PlayerData playerData { get; set; }
        public GameData gameData { get; set; }
        
        public void AddTOLevels(Level level);
        public void EditPlayedData(PlayerData playerData);

    }

//notification Channel    
    public class DataManger : MonoBehaviour, IData
    {


        public static DataManger instance;

        private void Awake()
        {
            instance = this;
        }

        private const string playerDataName = "Player";
        private const string gameDataName = "Game";
        public PlayerData playerData { get; set; }
        public GameData gameData { get; set; }
        public void AddTOLevels(Level level)
        {

            if (gameData.levels is null)
            {
                gameData.levels = new List<Level>();
                gameData.levels.Add(level);
            }
            else
            {
                var name = Int32.Parse(level.name);
                if (name<gameData.levels.Count)
                {

                    gameData.levels[name] = level;

                }
                else
                {
                    gameData.levels.Add(level);
                    
                }
                
            }
            
            
        }

        public void EditPlayedData(PlayerData playerData)
        {
            
        }

        



        private void Start()
        {
            LoadPlayerData();
            LoadGameData();
        }

        private  void LoadGameData()
        {
            var gameDataLoadResponse = Repository.current.LoadGameData(gameDataName);
            if (string.IsNullOrWhiteSpace(gameDataLoadResponse.error.message))
            {
                gameData = gameDataLoadResponse.data;
            }
            else
            {
                gameData = new GameData();
            }
        }

        private void LoadPlayerData()
        {
            var playerDatLoadResponse = Repository.current.LoadPlayerData(playerDataName);
            if (string.IsNullOrWhiteSpace(playerDatLoadResponse.error.message))
            {
                playerData = playerDatLoadResponse.data;
            }
            else
            {
                playerData = new PlayerData();
            }
        }

        private void OnDisable()
        {
            Repository.current.SavePlayerData(playerDataName, playerData);
            Repository.current.SaveGameData(gameDataName, gameData);
        }


    }
}