namespace GameSystems.Core
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using System;

    public class AssetLoaderService : MonoBehaviour,IAssetLoaderService
    {
        public void LoadAddressableAsset<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object
        {
            Addressables.LoadAssetAsync<T>(key).Completed += (AsyncOperationHandle<T> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    onLoaded?.Invoke(handle.Result);
                }
                else
                {
                    Debug.LogError($"Failed to load addressable asset with key {key}");
                    onLoaded?.Invoke(null);
                }
            };
        }

        public void InstantiateAddressablePrefab(string key, Action<GameObject> onInstantiated)
        {
            Addressables.LoadAsset<GameObject>(key).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = handle.Result;
                    
                    onInstantiated?.Invoke(prefab);
                    
                }
                else
                {
                    Debug.LogError($"Failed to instantiate addressable prefab with key {key}");
                    onInstantiated?.Invoke(null);
                }
            };
        }
    }


}