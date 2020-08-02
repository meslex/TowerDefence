using Camera_and_UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Camera_and_UI
{
    /// <summary>
    /// Maintains reference to currently displaying ui panel, and insures that they would not overlap
    /// </summary>
    public class UIManager : Singlenton<UIManager>
    {
        [SerializeField] private TowerOptionsUIController towerOptionsUI = default;
        [SerializeField] private TowerBuilderUIController towerBuilderUI = default;

        private BasePanelForTowerOptions currentPanel;

        public bool IsDisplaingPanel
        {
            get
            {
                if (currentPanel == null)
                    return false;
                else
                    return currentPanel.IsVisible;
            } 
        }

        public void SubscribeToBlockRaycastEvents(Action Lock, Action Unlock)
        {
            BlockRaycast[] uiPanels = transform.GetComponentsInChildren<BlockRaycast>();
            for(int i = 0; i < uiPanels.Length; ++i)
            {
                uiPanels[i].PointerEnter += Lock;
                uiPanels[i] .PointerExit += Unlock;
            }

        }

        private void Start()
        {
            if (towerOptionsUI == null || towerBuilderUI == null)
            {
                Debug.LogError("[UIManager] Panel were not initialised");
            }

            towerBuilderUI.OnTowerBuilded += SwitchPanels;

            MouseInput.Instance.OnEmptyTowerSpawnerClick += ShowBuilderPanel;
            MouseInput.Instance.OnOccupiedTowerSpawnerClick += ShowOptionsPanel;
        }

        public void HideCurrentPanel()
        {
            currentPanel.ClosePanel();
            //currentPanel = null;
        }

        /// <summary>
        /// Switch from builder to options panel
        /// </summary>
        /// <param name="ts"></param>
        public void SwitchPanels(TowerSpawner ts)
        {
            //ShowPanel(towerOptionsUI, ts);
            towerOptionsUI.SwitchPanel(ts);
            currentPanel = towerOptionsUI;
            towerBuilderUI.SwitchPanel();

            //((TowerBuilderUIController)currentPanel).SwitchPanel();
            //currentPanel = towerOptionsUI;
            //currentPanel?.ShowMenu(ts);

            //currentPanel = towerOptionsUI;

            //currentPanel?.ShowMenu(ts);
        }

        public void ShowPanel(BasePanelForTowerOptions panel, TowerSpawner ts)
        {
            if (currentPanel != null)
            {
                currentPanel.ClosePanel();
            }

            currentPanel = panel;
            currentPanel?.ShowMenu(ts);
        }


        private void ShowBuilderPanel(TowerSpawner ts)
        {
            ShowPanel(towerBuilderUI, ts);
        }

        private void ShowOptionsPanel(TowerSpawner ts)
        {
            ShowPanel(towerOptionsUI, ts);
        }
    }
}
