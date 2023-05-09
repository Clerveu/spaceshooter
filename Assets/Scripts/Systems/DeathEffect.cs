using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public GameObject deathAnimationPrefab;
    public float scale = 1.0f; // The scale factor for the instantiated prefab
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        if (health != null)
        {
            health.SubscribeToHealthChanged(OnHealthChanged);
        }
    }

    private void OnHealthChanged(float currentHealth)
    {
        if (currentHealth <= 0)
        {
            GameObject deathAnimation = Instantiate(deathAnimationPrefab, transform.position, Quaternion.identity);
            deathAnimation.transform.localScale = new Vector3(scale, scale, scale); // Set the scale of the instantiated prefab
        }
    }
}
