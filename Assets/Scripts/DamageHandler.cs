using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public Health health; // Reference to the Health script on the PlayerShip prefab
    public GameObject Explosion; // Reference to the Explosion prefab in the project folder
    public Shield shield;
    public GameObject ShieldDamage;

    private bool hasPlayedParticleEffect = false;

    private void Start()
    {
        // Get the Health component attached to the PlayerShip prefab
        health = GetComponent<Health>();
        shield = GetComponent<Shield>();
        // Subscribe to the OnHealthChanged event
        health.OnHealthChanged += HandleHealthChange;
        shield.OnShieldChanged += HandleShieldChange;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnHealthChanged event
        health.OnHealthChanged -= HandleHealthChange;
        shield.OnShieldChanged -= HandleShieldChange;
    }

    private void HandleHealthChange(float newHealth)
    {
        if (newHealth < health.maxHealth && !hasPlayedParticleEffect)
        {
            // Instantiate the Explosion prefab at the current position of the PlayerShip
            Instantiate(Explosion, transform.position, Quaternion.identity);
        }
    }

    private void HandleShieldChange(float newShield)
    {
        if (newShield < shield.maxShield && !hasPlayedParticleEffect)
        {
            // Instantiate the ShieldDamage object at the current position of the PlayerShip
            GameObject shieldDamageInstance = Instantiate(ShieldDamage, transform.position, Quaternion.identity);

            // Parent the ShieldDamage object to the PlayerShip, making it follow the PlayerShip
            shieldDamageInstance.transform.SetParent(transform);
        }
    }
}
