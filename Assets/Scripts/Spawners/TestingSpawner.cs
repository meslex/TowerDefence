using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSpawner : Singlenton<TestingSpawner>
{
    public GameObject prefab;
    public int amount;

    public event Action<float> OnEnemyDied;

    void Start()
    {
        StartCoroutine(Spawn()); 
    }

    //Spawns enemy to random waypoint
    private IEnumerator Spawn()
    {
        int count = 0;
        
        while(count < amount)
        {
            // Instantiate(prefab);
            GameObject obj = ObjectPooler.Instance.GetPooledObject(prefab);
            Transform child = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WaypointMovement>().SetWaypoint(child.GetComponent<Waypoint>());
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.Died += OnEnemyDied;
            obj.transform.position = child.position;
            obj.SetActive(true);

            yield return new WaitForEndOfFrame();

            count++;

        }
    }
}
