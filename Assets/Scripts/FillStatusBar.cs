using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{
    public Image fillImage;
    public Health health;
    private Slider slider;

    private void Start()
    {
        if (health == null)
        {
            GameObject playerShip = GameObject.FindGameObjectWithTag("PlayerShip"); // Replace "Player" with the appropriate tag for your PlayerShip object
            if (playerShip != null)
            {
                health = playerShip.GetComponent<Health>();
            }
        }

        slider = GetComponent<Slider>();
        if (health != null)
        {
            slider.minValue = 0f;
            slider.maxValue = health.maxHealth;
            slider.value = health.currentHealth;
            health.SubscribeToHealthChanged(UpdateSliderValue);
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.UnsubscribeFromHealthChanged(UpdateSliderValue);
        }
    }

    private void UpdateSliderValue(float healthValue)
    {
        slider.value = healthValue;
        fillImage.fillAmount = healthValue / health.maxHealth;
    }

    private void Update()
    {
        if (health.currentHealth > slider.value)
        {
            UpdateSliderValue(health.currentHealth);
        }
    }
}

