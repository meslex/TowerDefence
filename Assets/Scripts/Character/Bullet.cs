using Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;
    private float lifespan;

    public void Init(float speed,float damage, float lifespan, Vector3 direction)
    {
        this.speed = speed;
        this.damage = damage;
        this.transform.forward = direction;
        this.lifespan = lifespan;
        StartCoroutine(RemoveDelay());
    }

    private void OnEnable()
    {
        speed = 0;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if(damageable != null)
        {
            damageable.ReceiveDamage(damage);
        }
        gameObject.SetActive(false);
        ObjectPooler.Instance.ReturnObjectToPool(gameObject);

    }

    IEnumerator RemoveDelay()
    {
        yield return new WaitForSeconds(lifespan);

        gameObject.SetActive(false);
        ObjectPooler.Instance.ReturnObjectToPool(gameObject);
    }
}
