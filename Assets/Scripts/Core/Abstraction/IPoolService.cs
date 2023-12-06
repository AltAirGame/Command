using UnityEngine;

namespace GameSystems.Core
{
    public interface IPoolService
    {
        public GameObject Get(string tag);
        public GameObject GetGameObject();
    }
}