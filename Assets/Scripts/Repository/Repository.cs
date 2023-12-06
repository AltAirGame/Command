using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace GameSystems.Core
{
    public class Repository : MonoBehaviour, IRepository
    {
        private const string PlayerDataName = "Player.txt";
        private const string GameDataName = "Game.txt";


        [SerializeField] private IResourcesService resourcesService;

        private void Start()
        {
            InitializeServices();
        }

        public void InitializeServices()
        {


            resourcesService = ServiceLocator.Instance.GetService<IResourcesService>();
            if (resourcesService == null)
            {
                Debug.LogError("ResourcesService component is not attached to Repository.");
            }
        }

        public RepositoryResponse<LevelData> LoadGameData(string gameDataFilename)
        {
            return LoadData<LevelData>(GameDataName);
        }

        public RepositoryResponse<PlayerData> LoadPlayerData(string playerDataFilename)
        {
            return LoadData<PlayerData>(PlayerDataName);
        }

        public RepositoryResponse<LevelData> SaveGameData(string gameDataFilename, LevelData gameData)
        {
            return SaveData(gameDataFilename, gameData);
        }

        public RepositoryResponse<PlayerData> SavePlayerData(string playerDataFilename, PlayerData playerData)
        {
            return SaveData(playerDataFilename, playerData);
        }

        public RepositoryResponse<T> LoadData<T>(string saveDestination) where T : new()
        {
            var response = new RepositoryResponse<T>();
            var loadResponse = resourcesService.Load($"{saveDestination}");

            if (loadResponse.isSuccess)
            {
                response.data = JsonConvert.DeserializeObject<T>(loadResponse.body);
                response.error = new Error();
            }
            else
            {
                response.error = new Error(loadResponse.code, loadResponse.message);
            }

            return response;
        }

        public RepositoryResponse<T> SaveData<T>(string saveDestination, T data)
        {
            var response = new RepositoryResponse<T>();
            var saveSubject = JsonConvert.SerializeObject(data);
            var saveResponse = resourcesService.Save($"{saveDestination}", saveSubject);

            if (saveResponse.isSuccess)
            {
                response.data = data;
            }
            else
            {
                response.error = new Error(saveResponse.code, saveResponse.message);
            }

            return response;
        }
    }
}