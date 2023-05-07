using UnityEngine;
using UnityEngine.Events;

public class WeakPoint1Damage : MonoBehaviour
{
    public int weakPointNumber; // Set this in the inspector to 1 or 2, depending on the weak point
    private BossBehavior boss;
    private Health health;

    private void Start()
    {
        // Find the Boss1 script in the scene
        boss = FindObjectOfType<BossBehavior>();
        Debug.Log("WPD1 Found Boss Script");

        // Get the Health component and subscribe to the OnDamageTaken event
        health = GetComponent<Health>();
        health.OnDamageTaken += HandleDamageTaken;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnDamageTaken event when this object is destroyed
        if (health != null)
        {
            health.OnDamageTaken -= HandleDamageTaken;
        }
    }

    private void HandleDamageTaken(float damageAmount)
    {
        // Call the WeakPointTakeDamage method in the boss script when the weak point takes damage
        Debug.Log("Weak Point " + 1 + " taking damage: " + damageAmount);
        if (boss != null)
        {
            boss.WeakPointTakeDamage(1, damageAmount);
            Debug.Log("WPD1 logged " + damageAmount);
        }
    }
}
