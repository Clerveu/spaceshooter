using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCannonProjectile : MonoBehaviour
{
    public float damageAmount = 5f;
    public float projectileSpeed = 5f;

    private float maxDistance = 10f;
    private float traveledDistance = 0f;

    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.right * projectileSpeed * Time.deltaTime);

        // Keep track of the distance traveled
        traveledDistance += projectileSpeed * Time.deltaTime;

        // Check if the projectile has gone beyond the maximum distance
        if (traveledDistance >= maxDistance)
        {
            Destroy(gameObject); // Destroy the projectile
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().TakeDamage(damageAmount);
            Debug.Log("Cannon did" + damageAmount);
            Destroy(gameObject); // Destroy the projectile
        }
    }
}