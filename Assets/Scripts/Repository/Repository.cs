using System;
using Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


namespace MHamidi
{
    public class Repository : MonoBehaviour
    {

        public static Repository current;

        private void Awake()
        {
            current = this;
        }

        [SerializeField] private IResources _resources;

        private void Start()
        {
            _resources = GetComponent<IResources>();
        }

        public RepositoryResponse<GameData> LoadGameData(string saveDestination)
        {
            var levelCollectionResponse = new RepositoryResponse<GameData>();
            var response = _resources.Load(saveDestination);
            if (response.isSuccess)
            {
                levelCollectionResponse.data = new GameData(JObject.Parse(response.body));//Leveldata->Level
            }
            else
            {
                levelCollectionResponse.error = new Error(response.code, response.message);
            }

            return levelCollectionResponse;
        }
        public RepositoryResponse<PlayerData> LoadPlayerData(string saveDestination)
        {
            var levelCollectionResponse = new RepositoryResponse<PlayerData>();
            var response = _resources.Load(saveDestination);
            if (response.isSuccess)
            {
                levelCollectionResponse.data = new PlayerData(JObject.Parse(response.body));
            }
            else
            {
                levelCollectionResponse.error = new Error(response.code, response.message);
            }

            return levelCollectionResponse;
        }
        
        // There is A Problem Here
        // I Must Choose between Making 2 Model for the Level  Collection or Serialize my Level Manually  
        public RepositoryResponse<GameData> SaveEditorLevel(string saveDestination,GameData gameData)
        {
            var _response = new RepositoryResponse<GameData>();
            var saveSubject=JsonConvert.SerializeObject(gameData);//Level ->Level Data
            var response = _resources.Save(saveDestination,saveSubject);
            if (response.isSuccess)
            {
                _response.data = gameData;
                
            }
            else
            {
                
                _response.error = new Error(response.code,response.message); 
                
                
            }
            return _response;
        }
        public RepositoryResponse<PlayerData> SaveEditorLevel(string saveDestination,PlayerData playerData)
        {
            var _response = new RepositoryResponse<PlayerData>();
            var saveSubject=JsonConvert.SerializeObject(playerData);
            var response = _resources.Save(saveDestination,saveSubject);
            if (response.isSuccess)
            {
                _response.data = playerData;
            }
            else
            {
                _response.error = new Error(response.code, response.message);
            }
            return _response;
        }
    }
}