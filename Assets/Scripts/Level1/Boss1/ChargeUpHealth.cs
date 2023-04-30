using UnityEngine;

public class ChargeUpHealth : Health
{
    public BossBehavior bossBehavior;

    protected override void Die()
    {
        // Do something when this object dies
        bossBehavior.ChargeUpTakeDamage(maxHealth);
        Destroy(gameObject);
    }
}
