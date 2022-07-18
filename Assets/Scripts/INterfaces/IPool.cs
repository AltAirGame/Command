using UnityEngine;

namespace MHamidi.Helper
{
    public interface IPool
    {
        public GameObject Get(string tag);
        public GameObject GetGameObject();
    }
}