using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Enemies
{

    public class Enemy : Damageable
    {
        #region Fields

        public MeshRenderer mesh;
        public Slider slider;
        public Image fillImage;
        public Color fullHealthColor = Color.green;
        public Color zeroHealthColor = Color.red;
        public float timeBeforeBodyRemoval;

        //Position for tower to aim at
        public Vector3 Position { get { return mesh.transform.position; } }

        private WaypointMovement movement;
        private Collider enemyCollider;
        #endregion


        private void Awake()
        {
            movement = GetComponent<WaypointMovement>();
            enemyCollider = GetComponent<Collider>();
            movement.ReachedDestination += ReachedDestination;
        }

        /// <summary>
        /// Describes what enemy should do when its reached final waypoint
        /// </summary>
        protected virtual void ReachedDestination()
        {
            dead = true;
            gameObject.SetActive(false);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }

        protected virtual void OnEnable()
        {
            base.DamageRecieved += SetHealthUI;
            enemyCollider.enabled = true;

            Initialise();

            movement.StartMovement();

            slider.maxValue = initialHealth;
            slider.value = initialHealth;
            SetHealthUI();


        }

        protected virtual void OnDisable()
        {
            base.DamageRecieved -= SetHealthUI;
        }

        /// <summary>
        /// Displays current health
        /// </summary>
        private void SetHealthUI()
        {
            if (slider != null)
            {
                slider.value = currentHealth;

                fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / initialHealth);
            }
        }

        protected override void Die()
        {
            movement.Stop() ;
            enemyCollider.enabled = false;

            base.Die();
        }

    }

}

