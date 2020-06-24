using Spawners;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoneyHealth
{
    /// <summary>
    /// Responsible for all player health manipulation and triggers gameover 
    /// </summary>
    public class HealthController : Singlenton<HealthController>
    {
        public event Action GameOver;

        [SerializeField] private int initialHealth = default;
        [SerializeField] private Text HealthText = default;
        [SerializeField] private List<EnemySpawner> spawners = new List<EnemySpawner>();

        private int currentHealth;

        public int Health { get { return currentHealth; } }

        private void Start()
        {
            currentHealth = initialHealth;

            if (spawners.Count == 0)
            {
                Debug.LogError("[HealthController] no spawners were registered.");
            }

            for (int i = 0; i < spawners.Count; ++i)
            {
                spawners[i].OnEnemyReached += RemoveHealth;
            }

            ShowHealth();
        }

        public void AddHealth(int amount)
        {
            currentHealth += amount;
            ShowHealth();
        }

        public void RemoveHealth()
        {
            currentHealth--;
            ShowHealth();
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                GameOver?.Invoke();
            }

        }

        private void ShowHealth()
        {
            HealthText.text = $"HEALTH: {currentHealth}";
        }
    }
}
