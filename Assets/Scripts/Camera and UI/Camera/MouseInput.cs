using Camera_and_UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Responds to mouse clicks
    /// </summary>
    public class MouseInput : Singlenton<MouseInput>
    {
        [SerializeField] private LayerMask towerSpawnersLayer = default;

        public event Action<TowerSpawner> OnEmptyTowerSpawnerClick;
        public event Action<TowerSpawner> OnOccupiedTowerSpawnerClick;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                LookForTowerSpawner();
            }
        }

        private void LookForTowerSpawner()
        {
            RaycastHit hit;

            //Creates ray and check if it collides with tower spawner
            Ray ray = (Camera.main.ScreenPointToRay(Input.mousePosition));
            if (Physics.Raycast(ray, out hit, 200f, towerSpawnersLayer))
            {
                TowerSpawner ts = hit.collider.gameObject.GetComponent<TowerSpawner>();
                if(ts != null)
                {
                    if (ts.IsOccupied)
                    {
                        OnOccupiedTowerSpawnerClick?.Invoke(ts);
                    }
                    else
                    {
                        OnEmptyTowerSpawnerClick?.Invoke(ts);
                    }
                }
            }
                
        }
    }
}
