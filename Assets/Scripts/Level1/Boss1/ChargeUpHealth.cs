using UnityEngine;

public class ChargeUpHealth : Health
{
    public BossBehavior bossBehavior;

    private void Start()
    {
        bossBehavior = GetComponentInParent<BossBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is on the "Player" layer
        if (other.CompareTag("PlayerProjectile"))
        {
            bossBehavior.MySecretShame();
        }
    }

    protected override void Die()
    {
        // Do something when this object dies
        bossBehavior.ChargeUpTakeDamage(maxHealth);
        bossBehavior.OnChargeUpDestroyed(); // Call this method instead of setting the timer directly
        Destroy(gameObject);
    }
}
