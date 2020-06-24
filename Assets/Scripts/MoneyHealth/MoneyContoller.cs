using Spawners;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoneyHealth
{
    public class MoneyContoller : Singlenton<MoneyContoller>
    {
        public event Action<float> OnMoneyAmountChange; 
        
        [SerializeField] private Text moneyText = default;
        [SerializeField] private float initialMoney = default;
        [SerializeField] private List<EnemySpawner> spawners = new List<EnemySpawner>();
        private float currentMoney;
        private float score;
        private int enemiesKilled;

        public float Money { get { return currentMoney; } }
        public float Score { get { return score; } }
        public int EnemiesKilled { get { return enemiesKilled; } }


        // Start is called before the first frame update
        void Start()
        {
            if (spawners.Count == 0)
            {
                Debug.LogError("[MoneyContoller] no spawners were registered.");
            }

            for (int i =0; i < spawners.Count;++i)
            {
                spawners[i].OnEnemyDied += AddMoney;
            }

            currentMoney = initialMoney;
            UpdateHUD();   
        }

        public void AddMoney(float value)
        {
            currentMoney += value;
            score += value;
            enemiesKilled++;
            UpdateHUD();
        }

        public void RemoveMoney(float value)
        {
            currentMoney -= value;
            if (currentMoney < 0)
                currentMoney = 0;
            UpdateHUD();
        }

        private void UpdateHUD()
        {
            OnMoneyAmountChange?.Invoke(currentMoney);
            moneyText.text = $"MONEY: {currentMoney}";
        }
    }
}

