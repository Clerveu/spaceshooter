using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDamage : MonoBehaviour
{
    public Shield shield;
    public Health health;



    private void Start()
    {
        if (shield != null)
        {
            shield.SubscribeToShieldChanged(HandleShieldChanged);
        }
    }

    private void OnDestroy()
    {
        if (shield != null)
        {
            shield.UnsubscribeFromShieldChanged(HandleShieldChanged);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (shield != null && shield.currentShield > 0)
        {
            float remainingDamage = Mathf.Max(0, damageAmount - shield.currentShield);
            shield.TakeDamage(damageAmount);
            if (remainingDamage > 0)
            {
                health.TakeDamage(remainingDamage);
            }
        }
        else
        {
            health.TakeDamage(damageAmount);
        }
    }

    private void HandleShieldChanged(float currentShieldValue)
    {
        if (currentShieldValue <= 0)
        {
            shield.UnsubscribeFromShieldChanged(HandleShieldChanged);
        }
    }
}
