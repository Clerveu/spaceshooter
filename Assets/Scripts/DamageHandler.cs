using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public Health health; // Reference to the Health script on the PlayerShip prefab
    public GameObject Explosion; // Reference to the Explosion prefab in the project folder
    public Shield shield;
    public GameObject ShieldDamage;
    public float shakeAmount = 0.1f;
    public CameraShaker cameraShaker;

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
            GameObject healthDamageInstance = Instantiate(Explosion, transform.position, Quaternion.identity);
            AudioManager.instance.Play("hulldamage");
            cameraShaker.ShakeCamera(shakeAmount);
            // Parent the ShieldDamage object to the PlayerShip, making it follow the PlayerShip
            healthDamageInstance.transform.SetParent(transform);
        }
    }

    private void HandleShieldChange(float newShield)
    {
        if (newShield < shield.maxShield && !hasPlayedParticleEffect)
        {
            // Instantiate the ShieldDamage object at the current position of the PlayerShip
            GameObject shieldDamageInstance = Instantiate(ShieldDamage, transform.position + transform.right * 0.7f, Quaternion.identity);
            AudioManager.instance.Play("shielddamage");
            // Parent the ShieldDamage object to the PlayerShip, making it follow the PlayerShip
            shieldDamageInstance.transform.SetParent(transform);
        }
    }
}
