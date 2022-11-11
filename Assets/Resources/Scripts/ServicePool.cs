using System;
using Resources.Scripts.Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Scripts
{
    public class ServicePool : PoolingService<GameObject>
    {
        [SerializeField] private LevelChunkData[] levelChunks;

        public GameObject GetObject(string key)
        {
            return GetItem(key);
        }

        protected override GameObject CreateItem(string key)
        {
            
            var item = Array.Find(levelChunks,i => i.name == key);
            var pooledItem = Instantiate(item.levelChunks[Random.Range(0,item.levelChunks.Length)]);
            // pooledItem.SetActive(true);
            // pooledItem.transform.SetParent(this.transform);
            return pooledItem;
        }
    }
}