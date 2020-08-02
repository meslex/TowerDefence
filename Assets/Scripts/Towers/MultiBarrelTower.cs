using Enemies;
using System.Collections;
using System.Collections.Generic;
using Towers;
using Towers.Projectiles;
using UnityEngine;



public class MultiBarrelTower : Tower
{
    private const float PRECISION = 2f;
    [Range(0, 0.5f)]
    [SerializeField] protected float delayBetweenShots;
    [SerializeField] protected List<BarrelsLevelUps> barrelsLevelUps;
    [SerializeField] protected Barrel[] initialActiveBarrels;

    private Barrel[] currentActiveBarrels;
    private IEnumerator currentShootingCoroutine;

    protected override void OnEnable()
    {
        base.TargetIsOutOfRangeEvent += StopShoting;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.TargetIsOutOfRangeEvent -= StopShoting;
        base.OnDisable();
    }

    protected override void ResetTowerStats()
    {
        HideActiveBarrels();

        base.ResetTowerStats();
    }

    protected override void Init()
    {
        currentActiveBarrels = initialActiveBarrels;
        ShowActiveBarrels();
        base.Init();
    }

    #region Shoting
    protected override void Attack()
    {
        if (TurnToTarget())
        {
            Shot();
        }
    }

    private void StopShoting()
    {
        if (currentShootingCoroutine != null)
            StopCoroutine(currentShootingCoroutine);
    }

    protected virtual void Shot()
    {
        if (Time.time > nextAttack)
        {
            nextAttack = Time.time + AttackSpeed + delayBetweenShots * currentActiveBarrels.Length;

            currentShootingCoroutine = MultipleShot();
            StartCoroutine(currentShootingCoroutine);

        }
    }

    IEnumerator MultipleShot()
    {
        for(int i = 0; i < currentActiveBarrels.Length; ++i)
        {
            GameObject obj = ObjectPooler.Instance.GetPooledObject(projectile.gameObject);

            obj.GetComponent<Projectile>().Init(ProjectileSpeed, Damage, currentTarget);
            obj.transform.position = currentActiveBarrels[i].transform.position;
            obj.transform.forward = currentActiveBarrels[i].transform.forward;
            obj.SetActive(true);

            yield return new WaitForSeconds(delayBetweenShots);
        }

    }
    #endregion

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

    public override void Upgrade()
    {
        if (CanUpgrade)
        {
            ChangeActiveBarrels();

            base.Upgrade();
        }

    }

    private void ChangeActiveBarrels()
    {
        HideActiveBarrels();

        currentActiveBarrels = barrelsLevelUps[level].barrels;

        ShowActiveBarrels();
    }

    #region Show/HideBarrels
    private void ShowActiveBarrels()
    {
        for (int i = 0; i < currentActiveBarrels.Length; ++i)
        {
            currentActiveBarrels[i].Show();
        }
    }

    private void HideActiveBarrels()
    {
        for (int i = 0; i < currentActiveBarrels.Length; ++i)
        {
            currentActiveBarrels[i].Hide();
        }
    }
    #endregion
}
