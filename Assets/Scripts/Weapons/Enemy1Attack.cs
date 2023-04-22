using UnityEngine;

public class Enemy1Attack : MonoBehaviour
{
    public Transform target;
    public float projectileSpeed = 5f;
    public float damageAmount = 10f;
    public GameObject explosionEffectPrefab;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Calculate the direction towards the target
        Vector2 direction = (target.position - transform.position).normalized;

        // Apply force to the projectile to move it towards the target
        rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            other.GetComponent<ShieldDamage>().TakeDamage(damageAmount);
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject); // Destroy the projectile
        }
    }
}