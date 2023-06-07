using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool destroyedByPlayer = false;
    public event Action<float> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action OnDeath;

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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (OnDamageTaken != null)
        {
            OnDamageTaken.Invoke(damageAmount);
        }

        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(currentHealth);
        }

               
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke();
        destroyedByPlayer = true;
        Destroy(gameObject);
    }
}
