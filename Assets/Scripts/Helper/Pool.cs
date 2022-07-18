using System.Collections.Generic;
using UnityEngine;
using System.Collections;
namespace MHamidi.Helper
{
  

       [System.Serializable]
        public class PoolItem
        {
            public GameObject prefab;
            public int Amount;
            public bool IsExpandable;

        
        
        }

        public class Pool : MonoBehaviour, IPool
        {
            public List<PoolItem> items;
            public List<GameObject> PooledItems;
            private void Awake()
            {
                PopulateThePool();
            }
            public GameObject Get(string Id)
            {
            

                foreach (var item in PooledItems)
                {
                    if (item.activeInHierarchy is false&&item.CompareTag(Id))
                    {

                        return item;

                    }
                    else
                    {
                        /*Util.ShowMessag($" No Item In Pool with this Tag !");*/
                    }

                }
                foreach (var t in items)
                {
                    if (t.prefab.CompareTag(Id)&&t.IsExpandable)
                    {
                        GameObject obj = Instantiate(t.prefab);
                        PooledItems.Add(obj);
                        obj.SetActive(false);
                        return obj;
                    }
                }


                return null;
            }

            public GameObject GetGameObject()
            {
                return this.gameObject;
            }

            private void Start()
            {
                
            }

            private void PopulateThePool()
            {
                PooledItems = new List<GameObject>();

                for (int i = 0; i < items.Count; i++)
                {
                    for (int j = 0; j < items[i].Amount; j++)
                    {
                        GameObject obj = Instantiate(items[i].prefab,transform);
                        PooledItems.Add(obj);
                        obj.SetActive(false);
                    }
                }
            }
        }
    }
