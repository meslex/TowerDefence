using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Base;
using Enemies;

namespace Towers.Projectiles
{
    /// <summary>
    /// Base class for projectiles
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        protected float speed;
        protected float damage;
        protected Enemy target;

        [SerializeField] protected float lifeSpan;
        [SerializeField] protected float steeringForce;

        private Vector3 currentVelocity;

        private void OnEnable()
        {
            StartCoroutine(RemoveProjectile());
        }

        public virtual void Init(float speed, float damage, Enemy target)
        {
            this.speed = speed;
            this.damage  = damage;
            this.target = target;
        }

        private void Update()
        {
            Move();
        }

        protected virtual void Move()
        {
            Vector3 desiredVelocity = (target.Position - transform.position).normalized;
            Vector3 steering = (desiredVelocity - currentVelocity) * steeringForce;

            currentVelocity += steering;

            transform.position += currentVelocity.normalized * speed * Time.deltaTime;
        }

        protected virtual void OnTriggerEnter(Collider coll)
        {
            Debug.Log("Projectile collision");
            Damageable dm = coll.gameObject.GetComponent<Damageable>();
            if (dm != null)
            {
                dm.ReceiveDamage(damage);
            }
            gameObject.SetActive(false);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }

        
        private IEnumerator RemoveProjectile()
        {
            yield return new WaitForSeconds(lifeSpan);

            gameObject.SetActive(false);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }
    }
}
