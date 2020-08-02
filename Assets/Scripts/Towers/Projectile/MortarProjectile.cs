using Base;
using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using Towers.Projectiles;
using UnityEngine;

namespace Towers.Projectiles
{
    public class MortarProjectile : BaseProjectile
    {
        private Vector3 lastPos;
        private Vector3 impulse;
        private float gravity;
        [SerializeField] float explosionRadius = default;
        [SerializeField] float explosionForce = default;
        [SerializeField] LayerMask enemiesLayer = default;
        [SerializeField] GameObject explosionEffect = default;

        protected void FixedUpdate()
        {
            Move();
        }

        public void Init(float damage, BaseEnemy target, float gravity)
        {
            this.Target = target;
            this.Damage = damage;
            lastPos = transform.position;
            this.gravity = gravity;
            GetParticle();


        }

        private void GetParticle()
        {
            GameObject obj = ObjectPooler.Instance.GetPooledObject(explosionEffect.gameObject);
            explosionEffect = obj;
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        private float CalculateDamage(Vector3 targetPosition)
        {
            // Calculate the amount of damage a target should take based on it's position.
            Vector3 explosionToTarget = targetPosition - transform.position;

            float explosionDistance = explosionToTarget.magnitude;

            float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

            float damage = relativeDistance * Damage;

            damage = Mathf.Max(0f, damage);

            //Debug.Log($"Damage:{damage}");
            return damage;


        }

        private void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemiesLayer);

           // Debug.Log($"PROJECTILE has collided with: {colliders.Length} enemies");

            for (int i = 0; i < colliders.Length; ++i)
            {
                BaseEnemy trg = colliders[i].GetComponent<BaseEnemy>();

                if (trg == null)
                    continue;

                Vector3 impulse = new Vector3(trg.transform.position.x - transform.position.x, 0, trg.transform.position.z - transform.position.z);

                //Debug.Log($"Impulse: " + impulse);
                trg.Movement.AddImpulse(impulse.normalized * explosionForce);
                //trg.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);



                BaseEnemy enemy = colliders[i].GetComponent<BaseEnemy>();
                float damage = CalculateDamage(trg.transform.position);

                enemy.ReceiveDamage(damage);
            }

            explosionEffect.transform.position = transform.position;
            explosionEffect.gameObject.SetActive(true);



            col.enabled = false;
            moving = false;
            meshRenderer.enabled = false;
            StartCoroutine(DelayRemoval());
        }

        protected override void Move()
        {
            if (moving)
            {
                // Simple verlet integration
                float dt = Time.fixedDeltaTime;
                Vector3 accel = -gravity * Vector3.up;

                Vector3 curPos = transform.position;
                Vector3 newPos = curPos + (curPos - lastPos) + impulse * dt + accel * dt * dt;
                lastPos = curPos;
                transform.position = newPos;
                transform.forward = newPos - lastPos;

                impulse = Vector3.zero;

                // Z-kill
                if (transform.position.y < 1f)
                {

                    moving = false;
                    col.enabled = false;
                    Explode();
                    //gameObject.SetActive(false);
                    //ObjectPooler.Instance.ReturnObjectToPool(gameObject);
                }
            }
            
        }

        public void AddImpulse(Vector3 impulse)
        {
            this.impulse += impulse;
        }
    }

}
