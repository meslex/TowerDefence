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
        [SerializeField] private EnemySpawner[] spawners;

        private int currentHealth;

        public int Health { get { return currentHealth; } }

        private void Start()
        {
            currentHealth = initialHealth;

            spawners = EnemySpawnsController.Instance.Spawners;

            for (int i = 0; i < spawners.Length; ++i)
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
            //ShowHealth();
            if (currentHealth <= 0)
            {
                currentHealth = 0; 
                ShowHealth();
                GameOver?.Invoke();
            }
            ShowHealth();
        }

        private void ShowHealth()
        {
            HealthText.text = $"HEALTH: {currentHealth}";
        }
    }
}
