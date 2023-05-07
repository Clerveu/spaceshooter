using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLaser : MonoBehaviour
{
    public LineRenderer laserLineRenderer;
    public LayerMask laserHitLayers;
    public float laserTrackingSpeed = 20f;
    public float laserFireDuration = 3f;
    public float laserTargetingDelay = 1f;
    public float laserDamagePerSecond = 5f;
    public Transform laserParticleSystem;

    private bool isFiringLaser;
    private Transform target;
    private float laserFireTimer;
    private Vector3 initialFireDirection;
    private Quaternion targetRotation;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        Debug.Log("Target found: " + target != null);
        laserLineRenderer.enabled = false;
    }

    void Update()
    {
        if (isFiringLaser)
        {
            laserFireTimer += Time.deltaTime;
            if (laserFireTimer >= laserFireDuration)
            {
                StopFiringLaser();
            }
        }
        else
        {
            TrackPlayer();
        }
    }

    private void TrackPlayer()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, 0, angle + 90);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, laserTrackingSpeed * Time.deltaTime);

            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
            if (angleDifference < 5f) // Change the threshold from 1f to 5f
            {
                StartCoroutine(FireLaserAfterDelay(laserTargetingDelay));
            }
        }
    }

    private IEnumerator FireLaserAfterDelay(float delay)
    {
        initialFireDirection = (target.position - transform.position).normalized;
        yield return new WaitForSeconds(delay);
        transform.rotation = targetRotation;
        FireLaser();
    }

    private void FireLaser()
    {
        isFiringLaser = true;
        //laserLineRenderer.enabled = true;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, initialFireDirection, Mathf.Infinity, laserHitLayers);

        if (hit.collider != null)
        {
            laserParticleSystem.position = transform.position + initialFireDirection * (hit.distance * 0.5f);
            laserParticleSystem.localScale = new Vector3(laserParticleSystem.localScale.x, hit.distance * 0.5f, laserParticleSystem.localScale.z);

            if (hit.collider.CompareTag("PlayerShip"))
            {
                ShieldDamage shieldDamage = hit.collider.GetComponent<ShieldDamage>();
                if (shieldDamage != null)
                {
                    shieldDamage.TakeDamage(laserDamagePerSecond * Time.deltaTime);
                }
            }
        }
        else
        {
            laserParticleSystem.position = transform.position + initialFireDirection * (1000 * 0.5f);
            laserParticleSystem.localScale = new Vector3(laserParticleSystem.localScale.x, 1000 * 0.5f, laserParticleSystem.localScale.z);
        }
    }



    private void StopFiringLaser()
    {
        isFiringLaser = false;
        laserFireTimer = 0f;
        laserLineRenderer.enabled = false;
    }
}
