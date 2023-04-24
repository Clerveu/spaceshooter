using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAtPlayer : MonoBehaviour
{
    public float detectionRange = 5f;
    public float chargeDelay = 1f;
    public float chargeSpeed = 10f;
    public float chargeDistance = 5f;
    public float speedDecay = 0.1f;
    public float delayRotationSpeed = 100f;
    public float cooldownDuration = 1f;
    public float postChargeDelay = 1f; // Delay before the object can move again after the charge
    public EnergyBlade energyBlade;

    private GameObject player;
    private bool isCharging = false;
    private bool isOnCooldown = false;
    private Vector2 chargeDirection;
    private float chargeStartTime;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerShip");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null || isCharging || isOnCooldown)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            StartCoroutine(Charge());
        }
    }

    IEnumerator Charge()
    {
        isCharging = true;

        // Stop immediately
        rb.velocity = Vector2.zero;

        // Wait for chargeDelay while rotating towards the player
        float delayStartTime = Time.time;
        while (Time.time < delayStartTime + chargeDelay)
        {
            // Continuously rotate towards the player during the delay phase
            energyBlade.StartEnergyBlade();
            Vector2 direction = (Vector2)player.transform.position - (Vector2)transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = -rotateAmount * delayRotationSpeed;

            // Ensure the object remains stationary during the delay phase
            rb.velocity = Vector2.zero;

            yield return null;
        }

        // Charge towards player's last position and instantiate energy blade
        chargeDirection = (Vector2)player.transform.position - (Vector2)transform.position;
        chargeDirection.Normalize();
        chargeStartTime = Time.time;
        float currentSpeed = chargeSpeed;

        while (Time.time < chargeStartTime + (chargeDistance / chargeSpeed))
        {
            rb.velocity = chargeDirection * currentSpeed;
            currentSpeed -= speedDecay;
            yield return null;
        }

        // Stop after charging
        rb.velocity = Vector2.zero;

        // Reset the charging state
        isCharging = false;

        // Wait for postChargeDelay while holding position
        float postChargeStartTime = Time.time;
        while (Time.time < postChargeStartTime + postChargeDelay)
        {
            rb.velocity = Vector2.zero;
            yield return null;
        }

        // Activate the cooldown
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }
}

