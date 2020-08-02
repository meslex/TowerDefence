using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public abstract class BaseEnemy : Damageable
    {
        #region Fields
        public event Action DamageRecieved;
        public event Action<float> Died;

        protected bool dead;
        protected float currentHealth;

        [SerializeField] protected bool immortal;
        [SerializeField] protected float timeBeforeBodyRemoval;
        [SerializeField] protected MeshRenderer mesh;
        [SerializeField] protected EnemyStats stats;

        private HealthVisualizer healthBar;
        private WaypointMovement movement;
        private Collider enemyCollider;

        public EnemyStats Stats { get { return stats; } }
        public bool IsDead { get { return dead; } }
        public float Health { get { return currentHealth; } }
        public Vector3 Position { get { return mesh.transform.position; } }
        public WaypointMovement Movement { get { return movement; } }

        public List<Effect> effects = new List<Effect>();

        public float NormalisedHealth
        {
            get
            {
                if (Math.Abs(stats.MaxHealth) <= Mathf.Epsilon)
                {
                    Debug.LogError("Max Health is 0");
                    //stats.MaxHealth = 1f;
                }
                return currentHealth / stats.MaxHealth;
            }
        }
        #endregion

        private void Awake()
        {
            movement = GetComponent<WaypointMovement>();
            if (movement == null)
            {
                Debug.LogError("Enemy doesn't contain waypoint movement component!");
            }

            enemyCollider = GetComponent<Collider>();
            if (enemyCollider == null)
            {
                Debug.LogError($"Enemy lacks Collider Component. Enemy tag:{gameObject.tag}");
            }

            if (stats == null)
            {
                Debug.LogError($"[BaseEnemy]  enemy stat object wasn't assigned enemy tag:{gameObject.tag}");
            }

            movement.ReachedDestination += ReachedDestination;
        }

        protected virtual void ReachedDestination()
        {
            dead = true;
            gameObject.SetActive(false);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }

        protected virtual void OnEnable()
        {
            Initialise();
        }

        protected void Initialise()
        {
            enemyCollider.enabled = true;
            movement.Speed = stats.Speed;
            movement.StartMovement();
            currentHealth = stats.MaxHealth;
            dead = false;
        }

        public override void Heal(float amount)
        {
            currentHealth += amount;
            if (currentHealth > stats.MaxHealth)
                currentHealth = stats.MaxHealth;
        }

        public override void ReceiveDamage(float damage)
        {
            if (!dead && !immortal)
            {
                currentHealth -= damage;


                if (currentHealth <= 0f)
                {
                    currentHealth = 0;
                    DamageRecieved?.Invoke();
                    Die();
                }
                DamageRecieved?.Invoke();
            }
        }

        protected override void Die()
        {
            movement.Stop();

            //disable collider so dead enemy won't become a target for towers 
            enemyCollider.enabled = false;

            dead = true;
            Died?.Invoke(stats.Reward);
        }
    }
}
