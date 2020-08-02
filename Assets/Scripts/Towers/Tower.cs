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
        First,
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

        #region Protected properties
        protected TowerState currentState;
        protected BaseEnemy currentTarget;
        protected float nextAttack;

        protected int level;
        protected float currentRange;
        protected float currentAttackSpeed;
        protected float currentProjectileSpeed;
        protected float currentDamage;
        protected float currentRotationSpeed;
        protected float currentPrice;
        protected float currentSellPricePenalty;

        private Collider[] hitColliders;

        //[SerializeField] protected PrefabName towerName;
        [SerializeField] protected LayerMask enemyLayer;

        [SerializeField] protected TargetingOptions targetingType;
        [SerializeField] protected TowerStats stats;
        [SerializeField] protected GameObject pivotPoint;
        [SerializeField] protected BaseProjectile projectile;
        [SerializeField] protected Transform muzzle;


        #endregion

        #region Public properties
        /// <summary>
        /// Returns whether tower can upgrade
        /// </summary>
        public bool CanUpgrade
        {
            get
            {
                return level < stats.LevelUps.Count && stats.LevelUps[level].upgradePrice <= MoneyContoller.Instance.Money;
            }
        }

        /// <summary>
        /// Returns tower stats
        /// </summary>
        public TowerStats Stats { get { return stats; } }

        /// <summary>
        /// Returns tower target priority: closest/farthest etc.
        /// </summary>
        public TargetingOptions TargetPriority
        {
            get { return targetingType; }
            set { targetingType = value; }
        }

        /// <summary>
        /// Returns current tower target 
        /// </summary>
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// Returns price for next tower upgrade 
        /// </summary>
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

        /// <summary>
        /// Returns current tower's range 
        /// </summary>
        public float Range
        {
            get { return currentRange; }
            set { currentRange = value; }
        }

        /// <summary>
        /// Returns doubled range
        /// </summary>
        public float DoubledRange { get { return currentRange * 2; } }

        /// <summary>
        /// Return current attack speed
        /// </summary>
        public float AttackSpeed
        {
            get { return currentAttackSpeed; }
            set { currentAttackSpeed = value; }
        }

        /// <summary>
        /// Return current projectile speed
        /// </summary>
        public float ProjectileSpeed
        {
            get { return currentProjectileSpeed; }
            set { currentProjectileSpeed = value; }
        }

        /// <summary>
        /// Return current Damage
        /// </summary>
        public float Damage
        {
            get { return currentDamage; }
            set { currentDamage = value; }
        }

        /// <summary>
        /// Return current tower's rotation speed
        /// </summary>
        public float RotationSpeed
        {
            get { return currentRotationSpeed; }
            set { currentRotationSpeed = value; }
        }

        /// <summary>
        /// Return current tower's price
        /// </summary>
        public float Price
        {
            get { return currentPrice; }
            set { currentPrice = value; }
        }

        /// <summary>
        /// Returns amount that will be subtracted from value of tower after it was bought
        /// </summary>
        public float SellPricePenalty
        {
            get { return currentSellPricePenalty; }
            set { currentSellPricePenalty = value; }
        }
        #endregion

        protected virtual void OnEnable()
        {
            currentState = TowerState.LookingForTargets;

            Init();
        }

        protected virtual void OnDisable()
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

        /// <summary>
        /// Initialises tower with values from scriptable object
        /// </summary>
        protected virtual void Init()
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

        protected virtual bool LookForTargets()
        {
            hitColliders = Physics.OverlapSphere(transform.position, Range, enemyLayer);
            if (hitColliders.Length == 1)
            {
                currentTarget = hitColliders[0].gameObject.GetComponent<BaseEnemy>();

                if (currentTarget != null && !currentTarget.IsDead)
                {

                    return true;
                }

            }
            else if (hitColliders.Length >= 1)
            {
                int offset = 0;
                currentTarget = hitColliders[offset].GetComponent<BaseEnemy>();

                do
                {
                    currentTarget = hitColliders[offset].GetComponent<BaseEnemy>();
                    offset++;
                } while (currentTarget == null);

                BaseEnemy target;
                float currentDistance = Vector3.SqrMagnitude(muzzle.transform.position - currentTarget.Position);

                switch (targetingType)
                {
                    case TargetingOptions.First:

                        currentTarget = hitColliders[0].GetComponent<BaseEnemy>();

                        for (int i = offset; i < hitColliders.Length; ++i)
                        {
                            target = hitColliders[i].GetComponent<BaseEnemy>();
                            if (target != null && !target.IsDead && target.Movement.WaypointsSwitched > currentTarget.Movement.WaypointsSwitched)
                            {
                                currentTarget = target;

                            }
                        }
                        break;

                    case TargetingOptions.Closest:

                        for (int i = offset; i < hitColliders.Length; ++i)
                        {
                            target = hitColliders[i].GetComponent<BaseEnemy>();

                            float newDistance = Vector3.SqrMagnitude(muzzle.transform.position - target.transform.position);

                            if (target != null && !target.IsDead && newDistance < currentDistance)
                            {
                                currentTarget = target;
                                currentDistance = newDistance;
                            }
                        }
                        break;

                    case TargetingOptions.Farthest:
                        currentTarget = hitColliders[0].GetComponent<BaseEnemy>();

                        for (int i = offset; i < hitColliders.Length; ++i)
                        {
                            target = hitColliders[i].GetComponent<BaseEnemy>();
                            float newDistance = Vector3.SqrMagnitude(muzzle.transform.position - target.transform.position);

                            if (target != null && !target.IsDead && newDistance > currentDistance)
                            {
                                currentTarget = target;
                                currentDistance = newDistance;
                            }
                        }
                        break;

                    case TargetingOptions.Strongest:
                        currentTarget = hitColliders[0].GetComponent<BaseEnemy>();

                        for (int i = offset; i < hitColliders.Length; ++i)
                        {
                            target = hitColliders[i].GetComponent<BaseEnemy>();
                            if (target != null && !target.IsDead && target.Health > currentTarget.Health)
                            {
                                currentTarget = target;

                            }
                        }
                        break;

                    case TargetingOptions.Weakest:
                        currentTarget = hitColliders[0].GetComponent<BaseEnemy>();
                        for (int i = offset; i < hitColliders.Length; ++i)
                        {
                            target = hitColliders[i].GetComponent<BaseEnemy>();
                            if (target != null && !target.IsDead && target.Health < currentTarget.Health)
                            {
                                currentTarget = target;

                            }
                        }
                        break;

                    case TargetingOptions.Random:
                        break;
                }

                if (currentTarget != null && !currentTarget.IsDead)
                    return true;
                else
                    return false;
            }

            return false;
        }

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

        public virtual void Sell()
        {
            MoneyContoller.Instance.AddMoney(Price);
            gameObject.SetActive(false);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Range);
        }
    }

}

