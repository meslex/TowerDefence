using Base;
using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Towers.Projectiles
{
    public abstract class BaseProjectile : MonoBehaviour
    {
        #region Fields
        //private float speed;
        //private float damage;
        //private BaseEnemy target;

        protected Collider col;
        protected MeshRenderer meshRenderer;
        protected bool moving;
        [SerializeField] protected float lifeSpan;
        #endregion

        public float Speed { get; protected set; }
        public float Damage { get; protected set; }
        public BaseEnemy Target { get; protected set; }


        protected void Start()
        {
            col = GetComponent<Collider>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        protected virtual void OnEnable()
        {
            if(col != null)
                col.enabled = true;
            if(meshRenderer != null)
                meshRenderer.enabled = true;
            moving = true;
        }

        protected abstract void Move();




        protected virtual IEnumerator DelayRemoval()
        {
            yield return new WaitForSeconds(0.25f);
            gameObject.SetActive(false);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }

        protected virtual IEnumerator RemoveProjectile()
        {
            yield return new WaitForSeconds(lifeSpan);
            gameObject.SetActive(false);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }
    }
}
