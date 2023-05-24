using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{
    public Image fillImage;
    public Health health;
    private Slider slider;

    private bool isInitialized = false; // new variable to track if playerShip and slider have been found

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        {
            GameObject playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
            if (playerShip != null)
            {
                health = playerShip.GetComponent<Health>();
            }
        }

        slider = GetComponent<Slider>();
        if (health != null && slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = health.maxHealth;
            slider.value = health.currentHealth;
            health.SubscribeToHealthChanged(UpdateSliderValue);
            isInitialized = true; // components have been found successfully
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
        if (!isInitialized) // if components haven't been found, try to find them
        {
            InitializeComponents();
            return;
        }

        if (health.currentHealth > slider.value)
        {
            UpdateSliderValue(health.currentHealth);
        }
    }
}
