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
        bossBehavior.OnChargeUpDestroyed();
        AudioManager.instance.Play("bossinterrupt");
        AudioManager.instance.Stop("chargeup");
        Destroy(gameObject);
    }
}
