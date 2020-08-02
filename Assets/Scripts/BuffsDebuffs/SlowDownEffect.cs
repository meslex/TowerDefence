using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/SlowDownEffect")]
public class SlowDownEffect : Effect
{
    public override void ApplyEffect(BaseEnemy enemy, float amount, float duration = 0)
    {
        this.Amount = amount;
        active = true;
        enemy.Movement.Speed -= Amount;
    }

    public override bool CheckForAppliedEffects(BaseEnemy enemy)
    {
        return false;
    }

    public override void RemoveEffect(BaseEnemy enemy)
    {
        active = false;
        enemy.Movement.Speed += Amount;
    }
}
