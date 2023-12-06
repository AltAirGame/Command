using System;
using UnityEngine;

namespace GameSystems.Core
{
    public interface IAssetLoaderService
    {
        void LoadAddressableAsset<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object;
        void InstantiateAddressablePrefab(string key, Action<GameObject> onInstantiated);
    }
}