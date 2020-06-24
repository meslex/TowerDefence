using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesFinishController:MonoBehaviour
{
    public event Action<int> EnemyReachedFinish;
    [SerializeField] private int damageToPlayerPerEnemy = 1; 

    protected void OnTriggerEnter(Collider other)
    {
        Enemy enemy =  other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            EnemyReachedFinish?.Invoke(damageToPlayerPerEnemy);
            other.gameObject.SetActive(false);
            ObjectPooler.Instance.ReturnObjectToPool(other.gameObject);
        }
    }

}
