using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using Towers;

[System.Serializable]
public class TowerLevelUp
{
    public float upgradePrice;
    public float range;
    public float attackSpeed;
    public float projectileSpeed;
    public float damage;
    public float rotationSpeed;
    public float towerPrice;
}

[CreateAssetMenu(menuName = "Stats/TowerStats")]
public class TowerStats : ScriptableObject
{
    #region Fields
    [SerializeField] protected string towerName;

    [SerializeField] protected float initialRange;
    [SerializeField] protected float initialAttackSpeed;
    [SerializeField] protected float initialProjectileSpeed;
    [SerializeField] protected float initialDamage;
    [SerializeField] protected float initialRotationSpeed;
    [SerializeField] protected float initialPrice;
    [SerializeField] protected float initialSellPricePenalty;

    

    [SerializeField] protected List<TowerLevelUp> levelUps;
    #endregion

    #region Initial values properties
    public string TowerName { get { return towerName; } }

    public float InitialRange { get { return initialRange; } }
    public float InitialAttackSpeed { get { return initialAttackSpeed; } }
    public float InitialProjectileSpeed { get { return initialProjectileSpeed; } }
    public float InitialDamage { get { return initialDamage; } }
    public float InitialRotationSpeed { get { return initialRotationSpeed; } }
    public float InitialPrice { get { return initialPrice; } }
    public float InitialSellPricePenalty { get { return initialSellPricePenalty; } }
    public float DoubledInitialRange { get { return initialRange * 2; } }
    #endregion

    public List<TowerLevelUp> LevelUps { get { return levelUps; } }
}
