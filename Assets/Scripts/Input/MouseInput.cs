using Camera_and_UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Utils
{
    public class MouseInput : Singlenton<MouseInput>
    {
        [SerializeField] private LayerMask towerSpawnersLayer = default;

        public event Action<TowerSpawner> OnEmptyTowerSpawnerClick;
        public event Action<TowerSpawner> OnOccupiedTowerSpawnerClick;

        private bool raycastBlocked;
        private Camera mainCamera;

        private void Start()
        {
            UIManager.Instance.SubscribeToBlockRaycastEvents(BlockRaycast, UnlockRaycast);
            mainCamera = Camera.main;
        }

        private void BlockRaycast()
        {
            raycastBlocked = true;
        }

        private void UnlockRaycast()
        {
            raycastBlocked = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                //if we clicked outside panel while its displaying
                if(UIManager.Instance.IsDisplaingPanel && !raycastBlocked)
                {
                    UIManager.Instance.HideCurrentPanel();
                }

                if (!raycastBlocked)
                {
                    LookForTowerSpawner();
                }
            }
        }

        private void LookForTowerSpawner()
        {
            RaycastHit hit;
            //Fire ray from camera
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition),
                out hit, 1000f, towerSpawnersLayer))
            {
                TowerSpawner ts = hit.collider.gameObject.GetComponent<TowerSpawner>();
                if(ts != null)
                {
                    if (ts.IsOccupied)
                    {
                        OnOccupiedTowerSpawnerClick?.Invoke(ts);
                        //UIManager.Instance.ShowPanel(TowerOptionsUIController.Instance, ts);
                    }
                    else
                    {
                        OnEmptyTowerSpawnerClick?.Invoke(ts);
                        //UIManager.Instance.ShowPanel(TowerBuilderUIController.Instance, ts);
                    }
                }
            }
                
        }
    }
}
