using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Core
{
    [System.Serializable]
    public class PoolItem
    {
        public GameObject prefab;
        public int amount;
        public bool isExpandable;
    }

    public class PoolService : MonoBehaviour, IPoolService
    {
        [SerializeField]
        private List<PoolItem> poolItems = new List<PoolItem>();
        private Dictionary<string, List<GameObject>> pooledObjects = new Dictionary<string, List<GameObject>>();

        private void Awake()
        {
            InitializePool();
        }

        public GameObject Get(string tag)
        {
            if (!pooledObjects.TryGetValue(tag, out var objects))
            {
                Debug.LogError($"No pooled objects found with tag: {tag}");
                return null;
            }

            GameObject obj = objects.Find(item => !item.activeInHierarchy);
            if (obj != null)
            {
                return obj;
            }

            // Handle expandable pool items
            var poolItem = poolItems.Find(item => item.prefab.CompareTag(tag) && item.isExpandable);
            if (poolItem != null)
            {
                obj = Instantiate(poolItem.prefab, transform);
                obj.SetActive(false);
                objects.Add(obj);
                return obj;
            }

            Debug.LogError($"No available object in pool for tag: {tag} and pool is not expandable.");
            return null;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void InitializePool()
        {
            foreach (var item in poolItems)
            {
                var objectList = new List<GameObject>();
                for (int i = 0; i < item.amount; i++)
                {
                
                    
                    GameObject obj = Instantiate(item.prefab, transform);
                    obj.SetActive(false);
                    objectList.Add(obj);
                }


                if (item.prefab.CompareTag("Undefined"))
                {
                    LogError(item);
                }
                else
                {
                    LogDone();
                }

                
                pooledObjects.Add(item.prefab.tag, objectList);
            }
        }

        private static void LogDone()
        {
            Debug.Log("Done");
        }

        private static void LogError(PoolItem item)
        {
            Debug.LogError($"Remove this Item{item.prefab.name}");
        }
    }
}
