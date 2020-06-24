using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GroundEnemy : Enemy
{
    [SerializeField] protected Color skinColor;

    protected override void Die()
    {
        Remove();
        base.Die();
    }

    protected override void OnEnable()
    {
        mesh.material.color = skinColor;
        base.OnEnable();
    }

    //starts coroutine that will remove the body and paints mesh red
    protected virtual void Remove()
    {
        //navMeshAgent.isStopped = true;
        mesh.material.color = Color.red;
        StartCoroutine(RemoveBody());
    }

    /// <summary>
    /// Hides body after delay and disables gameobject
    /// </summary>
    /// <returns></returns>
    IEnumerator RemoveBody()
    {
        yield return new WaitForSeconds(timeBeforeBodyRemoval);

        //???
        //GetComponent<NavMeshAgent>().enabled = false;
        while (transform.position.y > -8)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -10, transform.position.z), 0.5f * Time.deltaTime);

            yield return null;
        }

        gameObject.SetActive(false);
        ObjectPooler.Instance.ReturnObjectToPool(gameObject);
    }
}
