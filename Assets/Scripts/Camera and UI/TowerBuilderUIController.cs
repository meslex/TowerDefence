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
    public TowerStats towerStats;
    public CustomButton button;
    public Text TowerName;
    public Text TowerPrice;
}


public class TowerBuilderUIController : BasePanelForTowerOptions
{
    [SerializeField] private List<Options> options = new List<Options>();



    private void OnEnable()
    {
        Hide();
        
    }

    private void SetText()
    {
        options[0].TowerName.text = $"{options[0].towerStats.TowerName}";
        options[0].TowerPrice.text = $"Tower price: {options[0].towerStats.InitialPrice}";
    }

    private void Start()
    {
        options[0].button.onClick.AddListener(FirstButtonPressed);
        options[0].button.OnHighlighted.AddListener(FirstButtonHighlighted);
        options[0].button.OnMouseExit.AddListener(FirstButtongRemoveHighlight);

        MoneyContoller.Instance.OnMoneyAmountChange += CheckPrices;

        targetingOptions.AddOptions(GetEnumValues(TargetingOptions.Closest));
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
            options[i].button.interactable = options[i].towerStats.InitialPrice <= currentMoney;
        }
    }

    public override void ShowMenu(TowerSpawner towerSpawner)
    {
        Show();
        currentSpawner = towerSpawner;
        SetText();
    }

    
    public virtual void FirstButtonPressed()
    {
        TowerRangeController.Instance.HideRange();
        currentSpawner.Spawn(options[0].towerStats, (TargetingOptions)targetingOptions.value);
        Hide();
    }

    public virtual void FirstButtonHighlighted()
    {
        TowerRangeController.Instance.ShowRange(currentSpawner, options[0].towerStats);
    }

    public virtual void FirstButtongRemoveHighlight()
    {
        TowerRangeController.Instance.HideRange();
    }

    public override void ClosePanel()
    {
        TowerRangeController.Instance.HideRange();
        currentSpawner = null;
        Hide();
    }

}
