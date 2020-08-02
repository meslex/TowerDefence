using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    MovespeedUp,
    SlowDown,
    FireDamage,
    Heal
}

/// <summary>
/// Damage Over Time
/// </summary>

public abstract class Effect : ScriptableObject
{
    [SerializeField] protected EffectType type = default;

    protected bool active;

    public EffectType Type { get { return type; } }
    public float Amount { get; protected set; }
    public GameObject PhysicalEffect { get; protected set; }
    public bool TimeBased { get; protected set; }
    public float Duration { get; protected set; }
    public bool Active { get { return active; } }

    public abstract bool CheckForAppliedEffects(BaseEnemy enemy);

    public abstract void ApplyEffect(BaseEnemy enemy, float amount, float duration = 0);

    public abstract void RemoveEffect(BaseEnemy enemy);
}
