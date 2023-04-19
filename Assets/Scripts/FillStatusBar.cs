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
            Debug.Log("PlayerShip GameObject located");
            if (playerShip != null)
            {
                health = playerShip.GetComponent<Health>();
                Debug.Log("PlayerShip Health Found");
            }
        }

        slider = GetComponent<Slider>();
        if (health != null)
        {
            slider.minValue = 0f;
            slider.maxValue = health.maxHealth;
            slider.value = health.currentHealth;
            Debug.Log("Slider set to" + health.currentHealth);
            health.SubscribeToHealthChanged(UpdateSliderValue);
            Debug.Log("FillStatusBar subscribed to OnHealthChanged event");
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.UnsubscribeFromHealthChanged(UpdateSliderValue);
            Debug.Log("FillStatusBar unsubscribed from OnHealthChanged event");
        }
    }

    private void UpdateSliderValue(float healthValue)
    {
        Debug.Log("UpdateSliderValue method called with health value: " + healthValue);
        slider.value = healthValue;
        fillImage.fillAmount = healthValue / health.maxHealth;
        Debug.Log("Slider value updated to: " + healthValue);
        Debug.Log("Slider value: " + slider.value);
        Debug.Log("Health value: " + healthValue);
    }
}

