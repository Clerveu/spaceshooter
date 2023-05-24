using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillShieldBar : MonoBehaviour
{
    public Image fillImage;
    public Shield shield;
    private Slider slider;

    private void Start()
    {
        {
            GameObject playerShip = GameObject.FindGameObjectWithTag("PlayerShip"); // Replace "Player" with the appropriate tag for your PlayerShip object
            if (playerShip != null)
            {
                shield = playerShip.GetComponent<Shield>();
            }
        }

        slider = GetComponent<Slider>();
        if (shield != null)
        {
            slider.minValue = 0f;
            slider.maxValue = shield.maxShield;
            slider.value = shield.currentShield;
            shield.SubscribeToShieldChanged(UpdateSliderValue);
        }
    }

    private void OnDestroy()
    {
        if (shield != null)
        {
            shield.UnsubscribeFromShieldChanged(UpdateSliderValue);
        }
    }

    private void UpdateSliderValue(float ShieldValue)
    {
        slider.value = ShieldValue;
        fillImage.fillAmount = ShieldValue / shield.maxShield;
    }
}

