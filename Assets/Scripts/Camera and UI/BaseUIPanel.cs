using Camera_and_UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Camera_and_UI
{
    /// <summary>
    /// Base panel that provides Show/Hide methods
    /// </summary>
    [RequireComponent(typeof(BlockRaycast))]
    public abstract class BaseUIPanel : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected CanvasGroup panel = default;

        //public event Action PointerEnter;
        //public event Action PointerExit;
        public bool IsVisible { get; private set; }


        /*public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit();
        }*/

        protected void Hide()
        {
            panel.alpha = 0f; //this makes everything transparent
            panel.blocksRaycasts = false; //this prevents the UI element to receive input events
            IsVisible = false;
        }

        protected void Show()
        {
            panel.alpha = 1f;
            panel.blocksRaycasts = true;
            IsVisible = true;
        }


    }
}
