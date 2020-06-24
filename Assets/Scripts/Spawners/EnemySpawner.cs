using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    [Serializable]
    public class Wave
    {
        [SerializeField] protected GameObject enemy;
        [SerializeField] protected int waveSize;
        [SerializeField] protected float spawnRate;
        [SerializeField] protected int maxWaveSize;
        [SerializeField] protected float minSpawnRate;
        [SerializeField] protected int waveSizeIncrement;
        [SerializeField] protected float spawnRateDecrement;

        public GameObject Enemy { get { return enemy; } }
        
        public int WaveSize
        {
            get { return waveSize; }
            private set
            { 
                waveSize = value;
                if (waveSize > maxWaveSize)
                    waveSize = maxWaveSize;
            }
        }
        
        public float SpawnRate
        {
            get { return spawnRate; }
            private set
            {
                spawnRate = value;
                if (spawnRate < minSpawnRate)
                    spawnRate = minSpawnRate;
            }
        }

        public void UpgradeWave()
        {
            WaveSize += waveSizeIncrement;
            SpawnRate -= spawnRateDecrement;
        }


    }

    /// <summary>
    /// Spawns waves of enemies
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] protected List<Wave> waves;
        [SerializeField] protected float wavesDelay;
        [SerializeField] protected float wavesDelayDecrement;
        [SerializeField] protected float minWavesDelay;
        [SerializeField] protected Waypoint start;

        private void DecreaseWavesDelay()
        {
            wavesDelay -= wavesDelayDecrement;
            if(wavesDelay < minWavesDelay)
            {
                wavesDelay = minWavesDelay;

            }
        }

        public event Action<float> OnEnemyDied;
        public event Action OnEnemyReached;

        private bool spawning;
        private IEnumerator coroutine;

        private void Start()
        {
            spawning = true;
            StartCoroutine(StartSpawning());
        }

        /// <summary>
        /// Creating waves
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartSpawning()
        {
            while (spawning)
            {
                Debug.Log("[EnemySpawner] Wave started");

                int coin = UnityEngine.Random.Range(0, waves.Count);
                coroutine = SpawnWave(waves[coin]);
                yield return StartCoroutine(coroutine);
                yield return new WaitForSeconds(wavesDelay);
                DecreaseWavesDelay();

                Debug.Log("[EnemySpawner] Wave ended");
            }
        }

        /// <summary>
        /// Spawning enemies 
        /// </summary>
        /// <param name="wave"></param>
        /// <returns></returns>
        private IEnumerator SpawnWave(Wave wave)
        {
            for(int i = 0; i < wave.WaveSize; ++i)
            {
                // Instantiate(prefab);
                GameObject obj = ObjectPooler.Instance.GetPooledObject(wave.Enemy);
                WaypointMovement wm = obj.GetComponent<WaypointMovement>();
                wm.SetWaypoint(start);
                wm.ReachedDestination += EnemyReachedDestination;
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.Died += EnemyDied;

                obj.transform.position = start.transform.position;
                obj.SetActive(true);

                yield return new WaitForSeconds(wave.SpawnRate);
            }

            wave.UpgradeWave();

        }

        private void EnemyDied(float amount)
        {
            OnEnemyDied?.Invoke(amount);
        }

        private void EnemyReachedDestination()
        {
            OnEnemyReached?.Invoke();
        }
    }
}
