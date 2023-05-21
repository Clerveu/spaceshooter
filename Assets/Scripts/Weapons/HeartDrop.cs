using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDrop : MonoBehaviour
{
    public GameObject particlePrefab;
    public float damageRadius = 5f;
    public float damageAmount = 50f;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

        if (health != null)
        {
            health.SubscribeToHealthChanged(OnHealthChanged);
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.UnsubscribeFromHealthChanged(OnHealthChanged);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            Explode(transform.position);
        }
    }

    private void OnHealthChanged(float currentHealth)
    {
        if (currentHealth <= 0)
        {
            Explode(transform.position);
        }
    }

    private void Explode(Vector3 position)
    {
        // Instantiate particle system prefab
        if (particlePrefab != null)
        {
            Instantiate(particlePrefab, position, Quaternion.identity);
        }

        // Apply AOE damage
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, damageRadius);
        foreach (Collider2D collider in colliders)
        {
            ShieldDamage shieldDamage = collider.GetComponent<ShieldDamage>();
            if (shieldDamage != null)
            {
                shieldDamage.TakeDamage(damageAmount);
            }
        }

        AudioManager.instance.Play("heart");
        Destroy(gameObject);
    }
}
