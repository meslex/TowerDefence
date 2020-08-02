using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAfterDelay : MonoBehaviour
{
    [SerializeField] private float delay = default;


    private void Start()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        ObjectPooler.Instance.ReturnObjectToPool(gameObject);
    }
}
