using Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


namespace MHamidi
{
    public class Repository : MonoBehaviour
    {
        [SerializeField] private IResources _resources;

        private void Start()
        {
            _resources = GetComponent<IResources>();
        }

        public RepositoryResponse<LevelCollection> LoadEditorLevel(string saveDestination)
        {
            var levelCollectionResponse = new RepositoryResponse<LevelCollection>();
            var response = _resources.Load(saveDestination);
            if (response.isSuccess)
            {
                levelCollectionResponse.data = new LevelCollection(JObject.Parse(response.body));
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
        public RepositoryResponse<LevelCollection> SaveEditorLevel(string saveDestination,LevelCollection levelCollection)
        {
            var _response = new RepositoryResponse<LevelCollection>();
            var saveSubject=JsonConvert.SerializeObject(levelCollection);
            var response = _resources.Save(saveDestination,saveSubject);
            if (response.isSuccess)
            {
                _response.data = levelCollection;
                
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