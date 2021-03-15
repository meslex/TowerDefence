using Enemies;
using MoneyHealth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Towers.Projectiles;
using UnityEngine;
using UnityEngine.UI;



//Change attack speed calculation it doesnt make sense at the moment
namespace Towers
{
   
    public enum TargetingOptions
    {
        Closest,
        Farthest,
        Weakest,
        Strongest,
        Random
    }

    public abstract class Tower : MonoBehaviour
    {
        public event Action TargetIsOutOfRangeEvent;
        public event Action TargetFoundEvent;

        protected enum TowerState
        {
            Attacking,
            LookingForTargets
        }

        #region protected properties
        protected TowerState currentState;
        protected Enemy currentTarget;
        protected float nextAttack;

        protected int level;
        protected float currentRange;
        protected float currentAttackSpeed;
        protected float currentProjectileSpeed;
        protected float currentDamage;
        protected float currentRotationSpeed;
        protected float currentPrice;
        protected float currentSellPricePenalty;

        [SerializeField] protected LayerMask enemyLayer;
        [SerializeField] protected TargetingOptions targetingType;
        [SerializeField] protected TowerStats stats;
        [SerializeField] protected GameObject pivotPoint;
        [SerializeField] protected GameObject muzzle;
        [SerializeField] protected GameObject projectile;

        #endregion

        #region Public properties

        public bool CanUpgrade
        {
            get
            {
                return level < stats.LevelUps.Count && stats.LevelUps[level].upgradePrice <= MoneyContoller.Instance.Money;
            }
        }

        public TowerStats Stats { get { return stats; } }

        public TargetingOptions TargetPriority
        {
            get { return targetingType; }
            set { targetingType = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public float UpgradePrice
        {
            get
            {
                if (level >= stats.LevelUps.Count)
                    return 0;
                else
                    return stats.LevelUps[level].upgradePrice;
            }
        }

        public float Range
        {
            get { return currentRange; }
            set { currentRange = value; }
        }

        public float DoubledRange { get { return currentRange * 2; } }

        public float AttackSpeed
        {
            get { return currentAttackSpeed; }
            set { currentAttackSpeed = value; }
        }

        public float ProjectileSpeed
        {
            get { return currentProjectileSpeed; }
            set { currentProjectileSpeed = value; }
        }

        public float Damage
        {
            get { return currentDamage; }
            set { currentDamage = value; }
        }

        public float RotationSpeed
        {
            get { return currentRotationSpeed; }
            set { currentRotationSpeed = value; }
        }

        public float Price
        {
            get { return currentPrice; }
            set { currentPrice = value; }
        }

        public float SellPricePenalty
        {
            get { return currentSellPricePenalty; }
            set { currentSellPricePenalty = value; }
        }
        #endregion

        protected virtual void OnEnable()
        {
            if (stats.TowerTag != gameObject.tag)
                Debug.LogError($"[Tower] tag from tower stats:{stats.TowerTag} " +
                    $"doesn't match with tower gameobject tag: {gameObject.tag}");

            Init();
        }

        private void OnDisable()
        {
            ResetTowerStats();

        }

        /// <summary>
        /// Reseting tower stats to default values
        /// </summary>
        protected virtual void ResetTowerStats()
        {
            currentState = TowerState.LookingForTargets;
            nextAttack = 0;
            Init();
        }


        void Update()
        {
            //simple fsm implementation
            switch (currentState)
            {
                case TowerState.LookingForTargets:
                    if (LookForTargets())
                    {
                        TargetFoundEvent?.Invoke();
                        currentState = TowerState.Attacking;
                    }

                    break;
                case TowerState.Attacking:
                    if (TargetIsOutOfRange() || currentTarget.IsDead)
                    {
                        TargetIsOutOfRangeEvent?.Invoke();
                        currentTarget = null;
                        currentState = TowerState.LookingForTargets;
                    }
                    else
                        Attack();
                    break;
            }
        }

        protected bool TargetIsOutOfRange()
        {
            float distance = Vector3.SqrMagnitude(transform.position - currentTarget.transform.position);
            if (distance > Range * Range)
                return true;
            else
                return false;
        }

        protected abstract void Attack();
        protected abstract bool LookForTargets();

        /// <summary>
        /// Upgrades tower
        /// </summary>
        /// <returns></returns>
        public virtual void Upgrade()
        {
            if (CanUpgrade)
            {
                MoneyContoller.Instance.RemoveMoney(UpgradePrice);

                currentRange += stats.LevelUps[level].range;
                currentAttackSpeed += stats.LevelUps[level].attackSpeed;
                currentProjectileSpeed += stats.LevelUps[level].projectileSpeed;
                currentDamage += stats.LevelUps[level].damage;
                currentRotationSpeed += stats.LevelUps[level].rotationSpeed;
                currentPrice += stats.LevelUps[level].towerPrice;
                level++;

            }
        }

        /// <summary>
        /// Initialises tower with values from scriptable object
        /// </summary>
        public void Init()
        {
            level = 0;
            currentRange = stats.InitialRange;
            currentAttackSpeed = stats.InitialAttackSpeed;
            currentProjectileSpeed = stats.InitialProjectileSpeed;
            currentDamage = stats.InitialDamage;
            currentRotationSpeed = stats.InitialRotationSpeed;
            currentPrice = stats.InitialPrice - stats.InitialSellPricePenalty;
            currentSellPricePenalty = stats.InitialSellPricePenalty;
        }


        public abstract void Sell();
    }

}

