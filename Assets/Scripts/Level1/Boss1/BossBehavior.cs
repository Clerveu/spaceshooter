using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public enum BossPhase { Spawn, Boss1phase1, Boss1phase1end, Boss1open, Boss1openprotected, Boss1phase2, Death }

    private Animator animator;
    private bool isCharging;
    private bool transitionInProgress;
    public BossPhase currentPhase;
    public GameObject enemy1AttackPrefab;

    [Header("Boss1phase1")]
    public float phase1DroneSpawnInterval = 2f;
    private float phase1DroneSpawnTimer;

    [Header("Movement")]
    public float speed = 2f;
    public float minY = -5.4f;
    public float maxY = 3f;
    public float acceleration = 20f;
    private Vector2 targetPosition;
    private Rigidbody2D rb;

    [Header("Charge Up Weapon")]
    public float chargeUpTime = 3f;
    public float chargeUpDamageThreshold = 10f;
    public GameObject chargeUpPrefab;
    public Transform chargeUpSpawnPoint;
    private GameObject currentChargeUp;
    private float chargeUpTimer;
    private float chargeUpDamageTaken;
    private int chargeUpDestroyedCount;

    [Header("Laser")]
    public LineRenderer laserLineRenderer;
    public float laserDamagePerSecond = 5f;
    public LayerMask laserHitLayers;
    private bool isFiringLaser;
    private float laserHitDistance;
    public GameObject bossBeamPrefab;
    private GameObject currentBossBeam;

    [Header("Boss1phase1end")]
    public float phase1EndTransitionTime = 1f;

    [Header("Boss1open")]
    public float openAnimationTime = 2f;

    [Header("Boss1openprotected")]
    public float protectedDuration = 5f;

    [Header("Boss1phase2")]
    public GameObject weakPointPrefab1;
    public GameObject weakPointPrefab2;
    public Transform weakPointSpawn1;
    public Transform weakPointSpawn2;
    private GameObject currentWeakPoint1;
    private GameObject currentWeakPoint2;
    public float sweepLaserDuration = 5f;
    public float sweepLaserSpeed = 2f;
    public float sweepLaserDelay = 1f;
    private float sweepLaserTimer;

    void Start()
    {
        animator = GetComponent<Animator>();
        phase1DroneSpawnTimer = phase1DroneSpawnInterval;
        rb = GetComponent<Rigidbody2D>();
        SetPhase(BossPhase.Spawn);
    }

    void Update()
    {
        switch (currentPhase)
        {
            case BossPhase.Boss1phase1:
                Phase1Update();

                chargeUpTimer -= Time.deltaTime;
                if (chargeUpTimer <= 0f && currentChargeUp == null && !isCharging)
                {
                    StartChargeUp();
                }

                if (isFiringLaser)
                {
                    FireLaser();
                }
                break;

            case BossPhase.Boss1phase1end:
                if (!transitionInProgress)
                {
                    transitionInProgress = true;
                    Debug.Log("Yup, transition's in progress, whatever that means");
                    StartCoroutine(TransitionToBoss1open());
                    Debug.Log("Starting TransitionToBoss1open coroutine");
                }
                break;

            case BossPhase.Boss1open:
                StartCoroutine(OpenAnimation());
                break;

            case BossPhase.Boss1openprotected:
                protectedDuration -= Time.deltaTime;
                if (protectedDuration <= 0f)
                {
                    SetPhase(BossPhase.Boss1phase2);
                }
                break;

            case BossPhase.Boss1phase2:
                Phase2Update();
                break;

            case BossPhase.Death:
                // Add death animation or any other logic here
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentPhase == BossPhase.Boss1phase1)
        {
            MoveToTargetPosition();
        }
    }

    private void Phase1Update()
    {
        phase1DroneSpawnTimer -= Time.deltaTime;
        if (phase1DroneSpawnTimer <= 0f)
        {
            SpawnDrone();
            phase1DroneSpawnTimer = phase1DroneSpawnInterval;
        }
    }

    private void Phase2Update()
    {
        sweepLaserTimer -= Time.deltaTime;
        if (sweepLaserTimer <= 0f)
        {
            StartCoroutine(SweepLaser());
            sweepLaserTimer = sweepLaserDuration + sweepLaserDelay;
        }
    }

    private void SetNewTargetPosition()
    {
        float newY = Random.Range(minY, maxY);
        targetPosition = new Vector2(transform.position.x, newY);
    }

    private void MoveToTargetPosition()
    {
        if (isFiringLaser)
        {
            GameObject player = GameObject.FindGameObjectWithTag("PlayerShip");
            if (player != null)
            {
                targetPosition = new Vector2(transform.position.x, player.transform.position.y + -1.2f);
            }
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetPosition);
        rb.AddForce(direction * acceleration, ForceMode2D.Force);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);

        if (distance < 0.1f && !isFiringLaser)
        {
            SetNewTargetPosition();
        }
    }

    private void SpawnDrone()
    { 
        Instantiate(enemy1AttackPrefab, transform.position, Quaternion.identity);
    }

    private void StartChargeUp()
    {
        currentChargeUp = Instantiate(chargeUpPrefab, chargeUpSpawnPoint.position, Quaternion.identity, transform);
        chargeUpTimer = chargeUpTime;
        chargeUpDamageTaken = 0f;
        StartCoroutine(ChargeUpCoroutine());
    }

    private IEnumerator ChargeUpCoroutine()
    {
        isCharging = true;
        yield return new WaitForSeconds(chargeUpTime);
        isCharging = false;

        if (currentChargeUp != null)
        {
            StartCoroutine(FireLaserAfterDelay(3f));
        }
        else
        {
            // Apply damage to the boss
        }
    }

    public void OnChargeUpDestroyed()
    {
        StopFiringLaser();
        chargeUpTimer = 5f;
    }


    private IEnumerator FireLaserAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentChargeUp != null)
        {
            FireLaser();
            yield return new WaitForSeconds(5); // Add a delay before stopping the laser
            StopFiringLaser();
        }
    }

    private void FireLaser()
    {
        isFiringLaser = true;
        laserLineRenderer.enabled = true;

        if (currentBossBeam == null)
        {
            currentBossBeam = Instantiate(bossBeamPrefab, chargeUpSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            currentBossBeam.transform.position = chargeUpSpawnPoint.position;
        }

        RaycastHit2D hit = Physics2D.Raycast(chargeUpSpawnPoint.position, Vector2.left, Mathf.Infinity, laserHitLayers);
        if (hit.collider != null)
        {
            laserHitDistance = hit.distance;

            laserLineRenderer.SetPosition(1, new Vector3(-laserHitDistance, 0, 0));

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
            laserLineRenderer.SetPosition(1, new Vector3(-1000, 0, 0));
        }
    }

    public void StopFiringLaser()
    {
        isFiringLaser = false;
        laserLineRenderer.enabled = false;

        if (currentBossBeam != null)
        {
            Destroy(currentBossBeam);
            currentBossBeam = null;
        }

        if (currentChargeUp != null)
        {
            Destroy(currentChargeUp);
            currentChargeUp = null;
            chargeUpTimer = 5f; // Reset the timer
        }
    }

    private IEnumerator StartChargeUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isCharging && currentChargeUp == null)
        {
            StartChargeUp();
        }
    }

    public void ChargeUpTakeDamage(float damage)
    {
        chargeUpDamageTaken += damage;

        if (chargeUpDamageTaken >= chargeUpDamageThreshold)
        {
            chargeUpDestroyedCount++;

            if (chargeUpDestroyedCount == 1)
            {
                SetPhase(BossPhase.Boss1phase1end);
            }
        }
    }

    private IEnumerator OpenAnimation()
    {
        yield return new WaitForSeconds(openAnimationTime);
        SetPhase(BossPhase.Boss1openprotected);
    }

    private IEnumerator SweepLaser()
    {
        // Implement the laser sweeping logic here
        yield return null;
    }


    public void OnSpawnAnimationEnd()
    {
        SetPhase(BossPhase.Boss1phase1);
    }

    IEnumerator TransitionToBoss1phase1()
    {
        yield return new WaitForSeconds(5.5f);
        //SetPhase(BossPhase.Boss1phase1);
    }

    IEnumerator TransitionToBoss1open()
    {
        float centerY = -1.25f;
        Vector2 targetPosition = new Vector2(transform.position.x, centerY);
        float acceleration = 5f;
        float maxSpeed = 2f;

        while (!Mathf.Approximately(transform.position.y, centerY))
        {
            Debug.Log("While loop entered, current centerY: " + centerY);

            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            float distance = Vector2.Distance(transform.position, targetPosition);

            rb.AddForce(direction * acceleration, ForceMode2D.Force);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

            if (distance < 0.1f)
            {
                break;
            }

            yield return null;
        }

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        Debug.Log("Rigidbody2D set to kinematic");

        yield return new WaitForSeconds(phase1EndTransitionTime);
        Debug.Log("Waited for phase1EndTransitionTime");
        SetPhase(BossPhase.Boss1open);
        transitionInProgress = false;
    }





    IEnumerator TransitionToBoss1openprotected()
    {
        yield return new WaitForSeconds(openAnimationTime);
        SetPhase(BossPhase.Boss1openprotected);
    }

    IEnumerator TransitionToBoss1phase2()
    {
        yield return new WaitForSeconds(protectedDuration);
        SetPhase(BossPhase.Boss1phase2);
    }

    private void ActivateWeakPoints()
    {
        if (currentWeakPoint1 == null)
        {
            currentWeakPoint1 = Instantiate(weakPointPrefab1, weakPointSpawn1.position, Quaternion.identity, transform);
        }

        if (currentWeakPoint2 == null)
        {
            currentWeakPoint2 = Instantiate(weakPointPrefab2, weakPointSpawn2.position, Quaternion.identity, transform);
        }
    }

    private void DeactivateWeakPoints()
    {
        if (currentWeakPoint1 != null)
        {
            Destroy(currentWeakPoint1);
            currentWeakPoint1 = null;
        }

        if (currentWeakPoint2 != null)
        {
            Destroy(currentWeakPoint2);
            currentWeakPoint2 = null;
        }
    }

    private void SetPhase(BossPhase newPhase)
    {
        currentPhase = newPhase;
        switch (currentPhase)
        {
            case BossPhase.Spawn:
                StartCoroutine(TransitionToBoss1phase1());
                break;

            case BossPhase.Boss1phase1:
                Debug.Log("Boss1phase1 Entered!");
                animator.enabled = false;
                Debug.Log("Animator disabled, god willing.");
                SetNewTargetPosition();
                break;

            case BossPhase.Boss1phase1end:
                // Remove the call to StartCoroutine(TransitionToBoss1open()); here
                break;

            case BossPhase.Boss1open:
                animator.enabled = true;
                animator.SetTrigger("Boss1open"); // Trigger the Open animation
                Debug.Log("animator.SetTrigger Boss1Open");
                StartCoroutine(TransitionToBoss1openprotected());
                Debug.Log("TransitionToBoss1openprotected Sent!");
                break;

            case BossPhase.Boss1openprotected:                
                StartCoroutine(TransitionToBoss1phase2());
                break;

            case BossPhase.Boss1phase2:
                ActivateWeakPoints();
                sweepLaserTimer = sweepLaserDuration;
                break;

            case BossPhase.Death:
                animator.SetTrigger("Death"); // Trigger the Death animation
                break;
        }
    }

}