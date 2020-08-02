using Enemies;
using System;
using UnityEngine;
using Utils;

[CreateAssetMenu(menuName = "Stats/WaveStats")]
public class WaveStats : ScriptableObject
{
    [SerializeField] protected bool boss;
    [SerializeField] protected BaseEnemy enemy;

    [SerializeField] protected int initialWaveSize;
    protected int waveSize;

    [SerializeField] protected float initialspawnRate;
    protected float spawnRate;

    [SerializeField] protected int upgradeRate;
    [SerializeField] protected int maxWaveSize;
    [SerializeField] protected float minSpawnRate;
    [SerializeField] protected int waveSizeIncrement;
    [SerializeField] protected float spawnRateDecrement;


    private int waveCounter;
    public BaseEnemy Enemy { get { return enemy; } }


    public void Init()
    {
        waveCounter = 0;
        waveSize = initialWaveSize;
        spawnRate = initialspawnRate;
        enemy.Stats.Init();

    }

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
        if (waveCounter > 0 && waveCounter % upgradeRate == 0)
            enemy.Stats.Upgrade();

        WaveSize += waveSizeIncrement;
        SpawnRate -= spawnRateDecrement;
        waveCounter++;
    }


}
