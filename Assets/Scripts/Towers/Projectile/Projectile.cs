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
    public class Projectile : BaseProjectile
    {

        [SerializeField] protected float steeringForce;

        private Vector3 currentVelocity;

        protected override void OnEnable()
        {
            StartCoroutine(RemoveProjectile());
            base.OnEnable();
        }

        public virtual void Init(float speed, float damage, BaseEnemy target)
        {
            this.Speed = speed;
            this.Damage = damage;
            this.Target = target;
        }

        protected void FixedUpdate()
        {
            if(moving)
                Move();
        }

        protected virtual void OnTriggerEnter(Collider coll)
        {
            Damageable dm = coll.gameObject.GetComponent<Damageable>();
            if (dm != null)
            {
                dm.ReceiveDamage(Damage);
            }

            col.enabled = false;
            moving = false;
            meshRenderer.enabled = false;
            StartCoroutine(DelayRemoval());
        }

        protected override void Move()
        {
            if (Target != null && !Target.IsDead)
            {
                Vector3 desiredVelocity = (Target.Position - transform.position).normalized;
                Vector3 steering = (desiredVelocity - currentVelocity) * steeringForce;

                currentVelocity += steering;

                transform.position += currentVelocity.normalized * Speed * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * Speed * Time.deltaTime;
                //Debug.Log("Projectile target is dead");
            }

            // Z-kill
            if (transform.position.y < -5f)
            {
                gameObject.SetActive(false);
                ObjectPooler.Instance.ReturnObjectToPool(gameObject);
            }


        }
    }
}
