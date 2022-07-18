using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


namespace MHamidi
{
    public class DataManger : MonoBehaviour, IData
    {



       

     

        private const string playerDataName = "Player.txt";
        private const string gameDataName = "Game.txt";
        public PlayerData playerData { get; set; }
        public GameData gameData { get; set; }
        public void AddTOLevels(Level level)
        {
            
            
            
            gameData ??= new GameData();
            gameData.levels ??= new List<Level>();

            var tempLevel=gameData.levels.Where(x => x.number == level.number).First();
            var index= gameData.levels.IndexOf(tempLevel);
            if (tempLevel is null)
            {
                gameData.levels.Add(level);
            }
            else
            {
                gameData.levels[index]= level;
            }

         
            Repository.current.SavePlayerData(playerDataName, playerData);
            Repository.current.SaveGameData(gameDataName, gameData);
            LoadGameData();
            
            
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