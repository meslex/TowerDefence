using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners.EnemySpawn
{
    public class WavesQueue : MonoBehaviour
    {
        [SerializeField] bool includeBoss = default;
        [SerializeField] protected WaveStats[] regularWaves;
        [SerializeField] protected WaveStats[] bossWaves;

        /// <summary>
        /// Size of wave pack, last one always will be the boss
        /// </summary>
        [SerializeField] protected int wavesPackSize;

        private Queue<WaveStats> waves = new Queue<WaveStats>();

        public int WavesCounter { get; private set; }

        private void Start()
        {
            for (int i = 0; i < regularWaves.Length; ++i)
                regularWaves[i].Init();

            for (int i = 0; i < bossWaves.Length; ++i)
                bossWaves[i].Init();

            StartCoroutine(FormRandomPack());
        }

        public WaveStats Dequeue()
        {
            WaveStats nextWave = waves.Dequeue();
            if (waves.Count == 0)
                StartCoroutine(FormRandomPack());

            nextWave.UpgradeWave();
            WavesCounter++;
            return nextWave;
        }



        private IEnumerator FormRandomPack()
        {
            if (includeBoss)
            {
                for (int i = 0; i < wavesPackSize - 1; ++i)
                {
                    waves.Enqueue(regularWaves[Random.Range(0, regularWaves.Length)]);


                    yield return new WaitForEndOfFrame();
                }

                waves.Enqueue(bossWaves[Random.Range(0, bossWaves.Length)]);
            }
            else
            {
                for (int i = 0; i < wavesPackSize; ++i)
                {
                    waves.Enqueue(regularWaves[Random.Range(0, regularWaves.Length)]);


                    yield return new WaitForEndOfFrame();
                }

            }


            //waves.Enqueue(bossWaves[Random.Range(0, bossWaves.Length)]);
        }
    }
}
