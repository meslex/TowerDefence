using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownStimulus : MonoBehaviour
{
    [SerializeField] private float amount= default; 


    private void OnTriggerEnter(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            Effect effect =  enemy.effects.Find(x => x.Type == EffectType.SlowDown);
            if (effect != null && !effect.Active)
            {
                effect.ApplyEffect(enemy, amount);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            Effect effect = enemy.effects.Find(x => x.Type == EffectType.SlowDown);
            if (effect != null && effect.Active)
            {
                effect.RemoveEffect(enemy);
            }
        }
    }
}
