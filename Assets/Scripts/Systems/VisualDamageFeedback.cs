using System.Collections;
using UnityEngine;

public class VisualDamageFeedback : MonoBehaviour
{
    public Material originalMaterial;
    public Material whiteMaterial;
    private Health health;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        health = GetComponent<Health>();
        if (health != null)
        {
            health.SubscribeToHealthChanged(OnHealthChanged);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;  // Store the original material
        }
    }

    void OnHealthChanged(float newHealth)
    {
        if (health == null)
        {
            return;
        }

        float oldHealth = health.currentHealth + 1;
        if (oldHealth > newHealth)
        {
            StartCoroutine(FlashSprite());
        }
    }

    IEnumerator FlashSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.material = whiteMaterial;  // Change to white material
            yield return new WaitForSeconds(0.025f);  // Wait for 0.1 seconds
            spriteRenderer.material = originalMaterial;  // Change back to the original material
        }
    }
}
