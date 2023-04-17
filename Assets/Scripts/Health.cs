using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public event Action<float> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log(gameObject.name + " has taken " + damageAmount + " damage. Current health: " + currentHealth);
    }

    private void Die()
    {
        // Do something when this object dies
        Destroy(gameObject);
        Debug.Log(gameObject.name + " has died.");
    }
}
