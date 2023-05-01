using UnityEngine;

public class ChargeUpHealth : Health
{
    public BossBehavior bossBehavior;

    private void Start()
    {
        bossBehavior = GetComponentInParent<BossBehavior>();
    }

    protected override void Die()
    {
        // Do something when this object dies
        bossBehavior.ChargeUpTakeDamage(maxHealth);
        bossBehavior.OnChargeUpDestroyed(); // Call this method instead of setting the timer directly
        Destroy(gameObject);
    }
}
