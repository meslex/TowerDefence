using UnityEngine;
using System.Collections;
using Towers.Projectiles;
using Enemies;
using MoneyHealth;

namespace Towers {

    public class BasicTower : Tower
    {
        protected const float PRECISION = 5f;

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

                GameObject obj = ObjectPooler.Instance.GetPooledObject(projectile.gameObject);

                if (obj != null)
                {
                    obj.GetComponent<Projectile>().Init(ProjectileSpeed, Damage, currentTarget);
                    obj.transform.position = muzzle.transform.position;
                    obj.transform.forward = muzzle.transform.forward;
                    obj.SetActive(true);
                }

            }
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
    }
}


