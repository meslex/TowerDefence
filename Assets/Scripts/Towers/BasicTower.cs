using UnityEngine;
using System.Collections;
using Towers.Projectiles;
using Enemies;
using MoneyHealth;

namespace Towers {

    public class BasicTower : Tower
    {
        private const float PRECISION = 5f;

        protected override void OnEnable()
        {
            currentState = TowerState.LookingForTargets;
            base.OnEnable();
        }

        protected override void Attack()
        {
            if (TurnToTarget())
            {
                Shoot();
            }
        }

        protected virtual void Shoot()
        {
            if (Time.time > nextAttack)
            {
                nextAttack = Time.time + AttackSpeed;

                GameObject obj = ObjectPooler.Instance.GetPooledObject(projectile);

                if (obj != null)
                {
                    obj.GetComponent<Projectile>().Init(ProjectileSpeed, Damage, currentTarget);
                    obj.transform.position = muzzle.transform.position;
                    obj.transform.forward = muzzle.transform.forward;
                    obj.SetActive(true);
                }

            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Range);
        }

        protected virtual bool TurnToTarget()
        {
            pivotPoint.transform.rotation = Quaternion.RotateTowards(pivotPoint.transform.rotation,
                Quaternion.LookRotation(currentTarget.Position - muzzle.transform.position), Time.deltaTime * RotationSpeed);

            Debug.DrawRay(muzzle.transform.position, currentTarget.Position - muzzle.transform.position, Color.red);

            float angle = Vector3.Angle(muzzle.transform.forward, currentTarget.Position - muzzle.transform.position);
            if (angle < PRECISION)
                return true;
            else
                return false;
        }

        protected override bool LookForTargets()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, Range, enemyLayer);
            if (hitColliders.Length == 1)
            {
                currentTarget = hitColliders[0].gameObject.GetComponent<Enemy>();

                if (currentTarget != null && !currentTarget.IsDead)
                {

                    return true;
                }

            }
            else if (hitColliders.Length >= 1)
            {
                int offset = 0;
                currentTarget = hitColliders[offset].GetComponent<Enemy>();

                do
                {
                    currentTarget = hitColliders[offset].GetComponent<Enemy>();
                    offset++;
                } while (currentTarget == null);

                Enemy target;
                float currentDistance = Vector3.SqrMagnitude(muzzle.transform.position - currentTarget.Position);

                switch (targetingType)
                {
                    case TargetingOptions.Closest:
                        
                        for (int i = offset; i < hitColliders.Length; ++i)
                        {
                            target = hitColliders[i].GetComponent<Enemy>();
                            
                            float newDistance = Vector3.SqrMagnitude(muzzle.transform.position - target.transform.position);

                            if (target != null && !target.IsDead && newDistance < currentDistance)
                            {
                                currentTarget = target;
                                currentDistance = newDistance;
                            }
                        }
                        break;

                    case TargetingOptions.Farthest:
                        currentTarget = hitColliders[0].GetComponent<Enemy>();

                        for (int i = offset; i < hitColliders.Length; ++i)
                        {
                            target = hitColliders[i].GetComponent<Enemy>();
                            float newDistance = Vector3.SqrMagnitude(muzzle.transform.position - target.transform.position);

                            if (target != null && !target.IsDead && newDistance > currentDistance)
                            {
                                currentTarget = target;
                                currentDistance = newDistance;
                            }
                        }
                        break;

                    case TargetingOptions.Strongest:
                        currentTarget = hitColliders[0].GetComponent<Enemy>();

                        for (int i = offset; i < hitColliders.Length; ++i)
                        {
                            target = hitColliders[i].GetComponent<Enemy>();
                            if (target != null && !target.IsDead && target.Health > currentTarget.Health)
                            {
                                currentTarget = target;

                            }
                        }
                        break;

                    case TargetingOptions.Weakest:
                        currentTarget = hitColliders[0].GetComponent<Enemy>();
                        for (int i = offset; i < hitColliders.Length; ++i)
                        {
                            target = hitColliders[i].GetComponent<Enemy>();
                            if (target != null && !target.IsDead && target.Health < currentTarget.Health)
                            {
                                currentTarget = target;

                            }
                        }
                        break;

                    case TargetingOptions.Random:
                        break;
                }

                if (currentTarget != null && !currentTarget.IsDead)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public override void Sell()
        {
            MoneyContoller.Instance.AddMoney(Price);
            gameObject.SetActive(false);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }
    }
}


