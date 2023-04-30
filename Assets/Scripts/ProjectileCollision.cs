using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public int damageAmount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().TakeDamage(damageAmount);
            Debug.Log("Projectile Collision did" + damageAmount);
        }
    }
}
