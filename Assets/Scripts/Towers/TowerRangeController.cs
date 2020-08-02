using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

public class TowerRangeController : Singlenton<TowerRangeController>
{
    private const float precision = 0.1f;

    [SerializeField] private MeshRenderer range = default;
    [Range(0, 1)]
    [SerializeField] private float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;
    private IEnumerator coroutine;

    private void OnEnable()
    {
        range.enabled = true;
    }

    /// <summary>
    /// Use this method when tower was already spawned
    /// </summary>
    /// <param name="tower"></param>
    public void ShowRange(Tower tower)
    {
        transform.position = new Vector3(tower.transform.position.x,
            tower.transform.position.y - 0.2f, tower.transform.position.z);;
        transform.localScale = Vector3.zero;

        StartResize(new Vector3(tower.DoubledRange, 0.01f, tower.DoubledRange));
    }

    /// <summary>
    /// This method is for displaying range of tower that wasn`t spawned yet
    /// </summary>
    /// <param name="spawner"></param>
    /// <param name="tower"></param>
    public void ShowRange(TowerSpawner spawner, Tower tower)
    {
        transform.position = new Vector3(spawner.transform.position.x,
            spawner.transform.position.y - 0.2f, spawner.transform.position.z);
        transform.localScale = Vector3.zero;

        StartResize(new Vector3(tower.Stats.DoubledInitialRange, 0.01f, tower.Stats.DoubledInitialRange));
    }

    /// <summary>
    /// This method is for displaying range of tower that wasn`t spawned yet
    /// </summary>
    /// <param name="spawner"></param>
    /// <param name="tower"></param>
    public void ShowRange(TowerSpawner spawner, TowerStats stats)
    {
        transform.position = new Vector3(spawner.transform.position.x,
            spawner.transform.position.y - 0.2f, spawner.transform.position.z);
        transform.localScale = Vector3.zero;

        StartResize(new Vector3(stats.DoubledInitialRange, 0.01f, stats.DoubledInitialRange));
    }

    IEnumerator ChangeSize(Vector3 targetScale)
    {
        for(float diff = Mathf.Abs(transform.localScale.x - targetScale.x); diff > precision; diff = Mathf.Abs(transform.localScale.x - targetScale.x))
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref velocity, smoothTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ResizeRange(Tower tower)
    {
        StartResize(new Vector3(tower.DoubledRange, 0.01f, tower.DoubledRange));
    }

    public void HideRange()
    {
        StartResize(Vector3.zero);
    }

    private void StartResize(Vector3 target)
    {
        //ensure that only one coroutine will work at a time
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = ChangeSize(target);
        StartCoroutine(coroutine);
    }
}
