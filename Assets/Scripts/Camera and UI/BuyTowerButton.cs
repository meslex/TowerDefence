using System;
using System.Collections;
using Towers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Camera_and_UI
{
    [Serializable]
    public class ButtonOnMouseEnter : UnityEvent { }
    [Serializable]
    public class ButtonOnMouseExit : UnityEvent { }


    /// <summary>
    /// Custom button that exposes OnPointerEnter/exit events
    /// </summary>
    public class BuyTowerButton : Selectable
    {
        [SerializeField] protected Tower tower;
        [SerializeField] protected Text TowerName;
        [SerializeField] protected Text TowerPrice;

        public Tower Tower { get { return tower; } }

        public event Action<Tower> OnClick;
        public event Action<Tower> OnHighlighted;
        public event Action OnMouseExit;

        protected override void Start()
        {
            SetText();
            base.Start();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (interactable)
            {
                Clicked();
                base.OnPointerDown(eventData);

            }

        }

        private void SetText()
        {
            TowerName.text = $"{tower.Stats.TowerName}";
            TowerPrice.text = $"Tower price: {tower.Stats.InitialPrice}";
        }

        private void Clicked()
        {
            OnClick?.Invoke(tower);
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            OnHighlighted?.Invoke(tower);
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            OnMouseExit?.Invoke();
            base.OnPointerExit(eventData);
        }
    }

}
