using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;

    public void GiveFullHealth(float health)
    {
        healthBarSlider.maxValue = health;        // sliderin alabileceði max can
        healthBarSlider.value = health;
    }

    public void SetHalth(float health) 
    {
        healthBarSlider.value = health;
    }
}
