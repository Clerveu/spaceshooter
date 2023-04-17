using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public Health health; // Reference to the Health script on the PlayerShip prefab
    public GameObject Explosion; // Reference to the Explosion prefab in the project folder

    private bool hasPlayedParticleEffect = false;

    private void Start()
    {
        // Get the Health component attached to the PlayerShip prefab
        health = GetComponent<Health>();

        // Subscribe to the OnHealthChanged event
        health.OnHealthChanged += HandleHealthChange;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnHealthChanged event
        health.OnHealthChanged -= HandleHealthChange;
    }

    private void HandleHealthChange(float newHealth)
    {
        if (newHealth < health.maxHealth && !hasPlayedParticleEffect)
        {
            // Instantiate the Explosion prefab at the current position of the PlayerShip
            Instantiate(Explosion, transform.position, Quaternion.identity);
        }
    }
}
