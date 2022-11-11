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
            var item = Array.Find(levelChunks,i => i.entryDirection.ToString() == key);
            var pooledItem = Instantiate(item.levelChunks[Random.Range(0,levelChunks.Length)]);
            // pooledItem.SetActive(true);
            // pooledItem.transform.SetParent(this.transform);
            return pooledItem;
        }
    }

    [Serializable]
    public struct ParticleEffect
    {
        public Keys key;
        public GameObject particleFxPrefab;
    }

    public enum Keys
    {
        Virus,
        AntiVirus,
        Smoke,
        PositiveGate,
        NegativeGate,
        Confetti
    }
}