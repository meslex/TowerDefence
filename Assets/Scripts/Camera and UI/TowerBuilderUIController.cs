using Camera_and_UI;
using MoneyHealth;
using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Towers.Tower;

[Serializable]
public class Options
{
    public Tower tower;
    //public CustomButton button;
    public Text TowerName;
    public Text TowerPrice;
}


public class TowerBuilderUIController : BasePanelForTowerOptions
{
    [SerializeField] private List<BuyTowerButton> options = new List<BuyTowerButton>();

    public event Action<TowerSpawner> OnTowerBuilded;

    private void OnEnable()
    {
        Hide();
        
    }

    private void Start()
    {
        for(int i = 0; i < options.Count; ++i)
        {
            options[i].OnClick += SpawnButtonPressed;
            options[i].OnHighlighted += ButtonHighlighted;
            options[i].OnMouseExit += ButtonRemoveHighlight;
        }

        MoneyContoller.Instance.OnMoneyAmountChange += CheckPrices;

        targetingOptions.AddOptions(GetEnumValues(TargetingOptions.Closest));
        CheckPrices(MoneyContoller.Instance.Money);
    }

    /// <summary>
    /// Checks which towers player can buy with current amount of money
    /// </summary>
    /// <param name="currentMoney"></param>
    private void CheckPrices(float currentMoney)
    {
        for(int i = 0; i < options.Count; ++i)
        {
            //if there is enough money to buy this tower make its button interactable
            options[i].interactable = options[i].Tower.Stats.InitialPrice <= currentMoney;
        }
    }

    public override void ShowMenu(TowerSpawner towerSpawner)
    {
        Show();
        targetingOptions.value = 0;
        currentSpawner = towerSpawner;
    }

    public virtual void SpawnButtonPressed(Tower ts)
    {
        currentSpawner.Spawn(ts, (TargetingOptions)targetingOptions.value);
        OnTowerBuilded?.Invoke(currentSpawner);
    }

    public virtual void ButtonHighlighted(Tower ts)
    {
        if (IsVisible)
            TowerRangeController.Instance.ShowRange(currentSpawner, ts);
    }

    public virtual void ButtonRemoveHighlight()
    {
        if(IsVisible)
            TowerRangeController.Instance.HideRange();
    }

    public override void ClosePanel()
    {
        TowerRangeController.Instance.HideRange();
        currentSpawner = null;
        Hide();
    }

    public override void SwitchPanel(TowerSpawner towerSpawner = null)
    {
        currentSpawner = null;
        Hide();
    }

}
