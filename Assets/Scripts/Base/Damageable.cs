using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public class Damageable : MonoBehaviour
    {
        public event Action DamageRecieved;
        public event Action<float> Died;

        [SerializeField] protected float initialHealth;
        [SerializeField] protected float reward;// reward for killing this enemy

        protected float currentHealth;
        protected bool dead;

        public bool IsDead { get { return dead; } }
        public float Health { get { return currentHealth; } }
        public float Reward { get { return reward; } }

        protected void Initialise()
        {
            currentHealth = initialHealth;
            dead = false;
        }

        public virtual void Heal(float amount)
        {
            currentHealth += amount;
            if (currentHealth > initialHealth)
                currentHealth = initialHealth;
        }

        public virtual void ReceiveDamage(float damage)
        {
            if (!dead)
            {
                currentHealth -= damage;
                DamageRecieved?.Invoke();

                if (currentHealth <= 0f)
                    Die();
            }
        }

        protected virtual void Die()
        {
            dead = true;
            Died?.Invoke(reward);
        }
    }
}

