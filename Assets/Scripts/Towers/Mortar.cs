using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Ballistic;
using Enemies;
using Towers.Projectiles;

namespace Towers
{
    public enum AimMode
    {
        Normal,
        Lateral
    }

    public class Mortar : Tower
    {
        private const float PRECISION = 2f;

        [SerializeField] private float gravity = default;
        [SerializeField] private float arcPeak = default;
        [SerializeField] private AimMode aimMode = default;

        private void Start()
        {
            float ballistic_range = fts.ballistic_range(ProjectileSpeed, gravity, muzzle.transform.position.y);
            Debug.Log($"[Mortar] ballistic_range: {ballistic_range}");
            if (currentRange > ballistic_range)
            {
                Debug.LogError("[Mortar] Current range exceeds possible ballistic range");
                currentRange = ballistic_range;
            }
        }

        private void LaunchProjectile(Vector3 fireVel)
        {
            GameObject proj = ObjectPooler.Instance.GetPooledObject(projectile.gameObject);
            MortarProjectile motion = proj.GetComponent<MortarProjectile>();

            proj.transform.position = muzzle.position;
            projectile.transform.forward = muzzle.forward;

            motion.Init(Damage, currentTarget, gravity);
            motion.AddImpulse(fireVel);
            proj.SetActive(true);

        }

        protected override void Attack()
        {
            Vector3 targetPos = currentTarget.Position;
            Vector3 diff = targetPos - muzzle.position;
            Vector3 diffGround = new Vector3(diff.x, 0f, diff.z);


            Vector3 fireVel, impactPos;
            if(aimMode == AimMode.Lateral)
            {
                if (fts.solve_ballistic_arc_lateral(muzzle.position, ProjectileSpeed, targetPos, currentTarget.Movement.Velocity, arcPeak, out fireVel, out gravity, out impactPos))
                {
                    if (TurnToTarget(fireVel))
                    {
                        if (Time.time > nextAttack)
                        {
                            LaunchProjectile(fireVel);
                            nextAttack = Time.time + AttackSpeed;
                        }
                    }

                }
            }
            else
            {
                Vector3[] solutions = new Vector3[2];
                int numSolutions;

                if (currentTarget.Movement.Velocity.sqrMagnitude > 0)
                    numSolutions = fts.solve_ballistic_arc(muzzle.position, ProjectileSpeed, targetPos, currentTarget.Movement.Velocity, gravity, out solutions[0], out solutions[1]);
                else
                    numSolutions = fts.solve_ballistic_arc(muzzle.position, ProjectileSpeed, targetPos, gravity, out solutions[0], out solutions[1]);

                if (numSolutions > 0)
                {
                    var impulse = solutions[1];
                    if (TurnToTarget(impulse))
                    {
                        if (Time.time > nextAttack)
                        {
                            LaunchProjectile(impulse);
                            nextAttack = Time.time + AttackSpeed;
                        }
                    }

                }
            }
            
        }

        private bool TurnToTarget(Vector3 fireVel)
        {
            pivotPoint.transform.rotation = Quaternion.RotateTowards(pivotPoint.transform.rotation,
            Quaternion.LookRotation(fireVel), Time.deltaTime * RotationSpeed);

            Debug.DrawRay(muzzle.transform.position, currentTarget.Position - muzzle.transform.position, Color.red);

            float angle = Vector3.Angle(muzzle.transform.forward, fireVel);
            if (angle < PRECISION)
                return true;
            else
                return false;
        } 

    }
}
