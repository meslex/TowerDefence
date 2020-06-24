using Camera_and_UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace Camera_and_UI
{
    /// <summary>
    /// Base panel that provides Show/Hide methods
    /// </summary>
    public abstract class BaseUIPanel : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup panel = default;


        protected void Hide()
        {
            panel.alpha = 0f; //this makes everything transparent
            panel.blocksRaycasts = false; //this prevents the UI element to receive input events
        }

        protected void Show()
        {
            panel.alpha = 1f;
            panel.blocksRaycasts = true;
        }


    }
}
