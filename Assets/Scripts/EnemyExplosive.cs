using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosive : MonoBehaviour
{
    public GameObject particlePrefab;
    public float damageRadius = 5f;
    public float damageAmount = 50f;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
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
            Health enemyHealth = collider.GetComponent<Health>();
            if (enemyHealth != null && collider.gameObject.CompareTag("Enemy"))
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }

    }
}
