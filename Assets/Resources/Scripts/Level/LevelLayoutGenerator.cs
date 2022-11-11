using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Scripts.Level
{
    public class LevelLayoutGenerator : MonoBehaviour
    {
        public LevelChunkData[] levelChunkData;
        public LevelChunkData firstChunk;

        private LevelChunkData _previousChunk;

        public Vector3 spawnOrigin;

        private Vector3 _spawnPosition;
        public int chunksToSpawn = 10;

        void OnEnable()
        {
            TriggerExit.OnChunkExited += PickAndSpawnChunk;
        }

        private void OnDisable()
        {
            TriggerExit.OnChunkExited -= PickAndSpawnChunk;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                PickAndSpawnChunk();
            }
        }

        void Start()
        {
            _previousChunk = firstChunk;

            for (int i = 0; i < chunksToSpawn; i++)
            {
                PickAndSpawnChunk();
            }
        }

        private LevelChunkData PickNextChunk()
        {
            _spawnPosition += new Vector3(0f, 0, _previousChunk.chunkSize.y);
            var allowedChunkList = levelChunkData.Where(i => i != _previousChunk).ToList();
            var nextChunk = allowedChunkList[Random.Range(0, allowedChunkList.Count)];
            return nextChunk;

        }

        void PickAndSpawnChunk()
        {
            LevelChunkData chunkToSpawn = PickNextChunk();

            GameObject objectFromChunk = chunkToSpawn.levelChunks[Random.Range(0, chunkToSpawn.levelChunks.Length)];
            _previousChunk = chunkToSpawn;
            Instantiate(objectFromChunk, _spawnPosition + spawnOrigin, Quaternion.identity);

        }

        public void UpdateSpawnOrigin(Vector3 originDelta)
        {
            spawnOrigin += originDelta;
        }

    }
}
