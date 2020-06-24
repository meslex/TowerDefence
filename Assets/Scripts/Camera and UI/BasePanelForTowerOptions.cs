using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace Camera_and_UI
{


    /// <summary>
    /// Base panel for panels showing tower information
    /// </summary>
    public abstract class BasePanelForTowerOptions : BaseUIPanel
    {
        [SerializeField] protected Dropdown targetingOptions;

        protected TowerSpawner currentSpawner;


        /// <summary>
        /// Return array with names of all enum values 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected List<string> GetEnumValues(System.Enum e)
        {
            List<string> enumValues = new List<string>();

            // Get all name-value pairs for incoming parameter.
            Array enumData = Enum.GetValues(e.GetType());

            //show the string name
            for (int i = 0; i < enumData.Length; i++)
            {
                enumValues.Add(enumData.GetValue(i).ToString());

            }

            return enumValues;

        }


        public abstract void ShowMenu(TowerSpawner towerSpawner);
        public abstract void ClosePanel();
    }



}
