using Enemies;
using Spawners.EnemySpawn;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public enum EnemySpawnerState
    {
        Spawning,
        Waiting
    }

    public class EnemySpawner : MonoBehaviour
    {
        public event Action<float> OnEnemyDied;
        public event Action OnEnemyReached;

        [SerializeField] protected float wavesDelay;
        [SerializeField] protected float wavesDelayDecrement;
        [SerializeField] protected float minWavesDelay;
        [SerializeField] protected Waypoint start;

        private WavesQueue wavesQueue;
        private IEnumerator spawnCoroutine;
        private IEnumerator delayCoroutine;
        private EnemySpawnerState currentState;


        public EnemySpawnerState State
        {
            get { return currentState; }
            private set
            {
                currentState = value;
                StateChanged();
            }
        }

        private void StateChanged()
        {
            if(State == EnemySpawnerState.Spawning)
            {
                if(delayCoroutine != null)
                    StopCoroutine(delayCoroutine);

                EnemySpawnsController.Instance.HideButton();
                spawnCoroutine = SpawnWave(wavesQueue.Dequeue());
                StartCoroutine(spawnCoroutine);
                DecreaseWavesDelay();
            }
            else
            {
                EnemySpawnsController.Instance.ShowButton();
                delayCoroutine = Delay();
                StartCoroutine(delayCoroutine);
            }
        }

        private void Start()
        {
            wavesQueue = GetComponent<WavesQueue>();
            if (wavesQueue == null)
            {
                Debug.LogError("[EnemySpawner] Couldn't find WavesQueue Component.");
                return;
            }

            EnemySpawnsController.Instance.StartWave += () => State = EnemySpawnerState.Spawning;
            State = EnemySpawnerState.Waiting;

        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(wavesDelay);

            State = EnemySpawnerState.Spawning;
        }

        private IEnumerator SpawnWave(WaveStats wave)
        {
            for(int i = 0; i < wave.WaveSize; ++i)
            {
                // Instantiate(prefab);
                GameObject obj = ObjectPooler.Instance.GetPooledObject(wave.Enemy.gameObject);

                BaseEnemy enemy = obj.GetComponent<BaseEnemy>();
                enemy.Died += EnemyDied;
                enemy.Movement.SetWaypoint(start);
                enemy.Movement.ReachedDestination += EnemyReachedDestination;
                obj.transform.position = start.transform.position;
                obj.SetActive(true);

                yield return new WaitForSeconds(wave.SpawnRate);
            }

            State = EnemySpawnerState.Waiting;
        }

        private void EnemyDied(float amount)
        {
            OnEnemyDied?.Invoke(amount);
        }

        private void EnemyReachedDestination()
        {
            OnEnemyReached?.Invoke();
        }

        private void DecreaseWavesDelay()
        {
            wavesDelay -= wavesDelayDecrement;
            if (wavesDelay < minWavesDelay)
            {
                wavesDelay = minWavesDelay;

            }
        }
    }
}
