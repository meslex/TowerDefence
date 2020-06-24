using System;
using System.Collections;
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
    public class CustomButton : Button
    {
        public ButtonOnMouseEnter OnHighlighted;
        public ButtonOnMouseExit  OnMouseExit;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            OnHighlighted?.Invoke();
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            OnMouseExit?.Invoke();
            base.OnPointerExit(eventData);
        }
    }

}
