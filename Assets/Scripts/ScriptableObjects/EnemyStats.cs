using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] protected string enemyName = default;
   
    [SerializeField] private float initialHealth = default;
    [SerializeField] private float initialSpeed = default;
    [SerializeField] private float maxSpeed = default;
    [SerializeField] private int initialReward = default;
    [SerializeField] private float speedIncrement = default;
    [SerializeField] private float healthIncrement = default;
    [SerializeField] private int rewardIncrement = default;

    private float maxHealth = default;
    private float speed = default;
    private int reward = default;

    public string EnemyName { get { return enemyName; } }
    public float InitialHealth { get { return initialHealth; } }
    public float Speed
    {
        get { return speed; }
        private set
        {
            speed = value;
            if (speed > maxSpeed)
                speed = maxSpeed;
        }
    } 
    public float MaxHealth { get { return maxHealth; } }
    public int Reward { get { return reward; } }

    public void Init()
    {
        maxHealth = initialHealth;
        speed = initialSpeed;
        reward = initialReward;
    }

    public void Upgrade()
    {
        Debug.Log($"Upgrading enemy: {enemyName}");
        maxHealth += healthIncrement;
        Speed += speedIncrement;
        reward += rewardIncrement;
    }
}
