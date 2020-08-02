using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    #region HealthSliderFields
    public Slider slider;
    public Image fillImage;
    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;
    #endregion

    /// <summary>
    /// Displays current health
    /// </summary>
    public void SetHealthUI(EnemyStats stats, float currentHealth)
    {
        if (slider != null)
        {
            slider.maxValue = stats.MaxHealth;
            slider.value = currentHealth;
            fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / stats.MaxHealth);
        }
    }
}
