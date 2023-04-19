using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public event Action<float> OnHealthChanged;

    public void SubscribeToHealthChanged(Action<float> method)
    {
        OnHealthChanged += method;
    }

    public void UnsubscribeFromHealthChanged(Action<float> method)
    {
        OnHealthChanged -= method;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (OnHealthChanged != null)
        {
            Debug.Log("OnHealthChanged event has subscribers.");
            OnHealthChanged.Invoke(currentHealth);
            Debug.Log("OnHealthChanged event invoked with health value: " + currentHealth);
        }

        else
        {
            Debug.Log("OnHealthChanged event has no subscribers.");
        }

        Debug.Log("Health Changed, currentHealth should be invoked.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Do something when this object dies
        Destroy(gameObject);
    }
}
