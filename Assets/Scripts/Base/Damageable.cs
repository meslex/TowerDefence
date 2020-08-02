using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public abstract class Damageable : MonoBehaviour
    {
        public abstract void Heal(float amount);
        public abstract void ReceiveDamage(float damage);
        protected abstract void Die();

    }
}

