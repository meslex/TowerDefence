using Camera_and_UI;
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

        private void Start()
        {
            if (towerOptionsUI == null || towerBuilderUI == null)
            {
                Debug.LogError("[UIManager] Panel were not initialised");
            }

            MouseInput.Instance.OnEmptyTowerSpawnerClick += ShowBuilderPanel;
            MouseInput.Instance.OnOccupiedTowerSpawnerClick += ShowOptionsPanel;
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
