using Camera_and_UI;
using MoneyHealth;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;
using UnityEngine.UI;
using static Towers.Tower;

//Add upgrade button block
public class TowerOptionsUIController : BasePanelForTowerOptions
{
    
    [SerializeField] private Text currentLevelText = default;
    [SerializeField] private Text towerNameText = default;
    [SerializeField] private Text towerUpgradePriceText = default;
    [SerializeField] private Text towerSellPriceText = default;
    [SerializeField] private Button UpgradeButton = default;
    
    private void Start()
    {
        targetingOptions.AddOptions(GetEnumValues(TargetingOptions.Closest));
        MoneyContoller.Instance.OnMoneyAmountChange += CheckIfTowerCanBeUpgraded;
    }

    private void OnEnable()
    {
        Hide();
    }

    private void CheckIfTowerCanBeUpgraded(float money)
    {
        if (currentSpawner != null)
        {
            UpgradeButton.interactable = currentSpawner.CurrentTower.CanUpgrade;
        }
    }

    public override void ShowMenu(TowerSpawner ts)
    {
        currentSpawner = ts;
        targetingOptions.value = (int)currentSpawner.CurrentTower.TargetPriority;
        TowerRangeController.Instance.ShowRange(currentSpawner.CurrentTower);
        currentLevelText.text = $"Level: {currentSpawner.CurrentTower.Level}";
        towerNameText.text = currentSpawner.CurrentTower.Stats.TowerName;
        SetPrices();
        UpgradeButton.interactable = currentSpawner.CurrentTower.CanUpgrade;
        Show();
    }

    public override void SwitchPanel(TowerSpawner towerSpawner)
    {
        currentSpawner = towerSpawner;
        targetingOptions.value = (int)currentSpawner.CurrentTower.TargetPriority;
        
        currentLevelText.text = $"Level: {currentSpawner.CurrentTower.Level}";
        towerNameText.text = currentSpawner.CurrentTower.Stats.TowerName;
        SetPrices();
        UpgradeButton.interactable = currentSpawner.CurrentTower.CanUpgrade;
        Show();
    }

    /*public virtual void ShowMenuAfterBuilderUI(TowerSpawner ts)
    {
        currentSpawner = ts;
        targetingOptions.value = (int)currentSpawner.CurrentTower.TargetPriority;
        currentLevelText.text = $"Level: {currentSpawner.CurrentTower.Level}";
        towerNameText.text = currentSpawner.CurrentTower.Stats.TowerName;
        SetPrices();
        UpgradeButton.interactable = currentSpawner.CurrentTower.CanUpgrade;
        Show();
    }*/

    private void SetPrices()
    {
        towerUpgradePriceText.text = $"Upgrade price: {currentSpawner.CurrentTower.UpgradePrice}";
        towerSellPriceText.text = $"Sell price: {currentSpawner.CurrentTower.Price}";
    }

    public void OnUpgradeClick()
    {

        if (currentSpawner.CurrentTower.CanUpgrade)
        {
            currentSpawner.CurrentTower.Upgrade();
            TowerRangeController.Instance.ResizeRange(currentSpawner.CurrentTower);
            currentLevelText.text = $"Level: {currentSpawner.CurrentTower.Level}";
            SetPrices();
            if (!currentSpawner.CurrentTower.CanUpgrade)
            {
                UpgradeButton.interactable = false;
            }
        }
    }

    public void OnSellClick()
    {
        currentSpawner.FreeSpawnPosition();
        currentSpawner.CurrentTower.Sell();
        ClosePanel();
    }

    public void OnTargetingTypeChanged()
    {
        currentSpawner.CurrentTower.TargetPriority = (TargetingOptions)targetingOptions.value;
        //Debug.Log($"[TowerOptionsController] tower's target priority was changed. Current target priority: {currentSpawner.CurrentTower.TargetPriority}");
    }

    public override void ClosePanel()
    {
        TowerRangeController.Instance.HideRange();
        currentSpawner = null;
        Hide();
    }
}
