using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class Poolable : MonoBehaviour
    {
        private int hashedName;

        public int HashedName { get { return hashedName; } }

        private void Awake()
        {
            hashedName = gameObject.name.GetHashCode();
        }
    }
}
