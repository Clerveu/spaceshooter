using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public enum BossPhase { Spawn, Boss1phase1, Boss1phase1end, Boss1open, Boss1openprotected, Boss1phase2, Death, Destruction }

    public BossPhase currentPhase;
    private Animator animator;
    private bool transitionInProgress;
    private bool enteredPhase2 = false;
    public GameObject bossDronePrefab;

    [Header("Boss1phase1")]
    public float phase1DroneSpawnInterval = 2f;
    private float phase1DroneSpawnTimer;
    public Transform droneSpawnPointPhase1;
    public float speed = 2f;
    public float minY = -5.4f;
    public float maxY = 3f;
    public float acceleration = 20f;
    private Vector2 targetPosition;
    private Rigidbody2D rb;

    [Header("Charge Up")]
    private bool isCharging;
    private int chargeUpDestroyedCount;
    public float chargeUpTime = 3f;
    public float chargeUpDamageThreshold = 10f;
    private float chargeUpTimer;
    private float chargeUpDamageTaken;
    public GameObject chargeUpPrefab;
    private GameObject currentChargeUp;
    public Transform chargeUpSpawnPoint;
    public Transform chargeUpFeedbackPoint;

    [Header("Laser")]
    private bool isFiringLaser;
    public float laserDamagePerSecond = 5f;
    private float laserHitDistance;
    public GameObject bossBeamPrefab;
    private GameObject currentBossBeam;
    public LineRenderer laserLineRenderer;
    public LayerMask laserHitLayers;

    [Header("Boss1phase1end")]
    public float phase1EndTransitionTime = 1f;

    [Header("Boss1open")]
    public float openAnimationTime = 2f;

    [Header("Boss1openprotected")]
    public float protectedDuration = 5f;
    public float protectedDurationMax = 10f;
    public float protectedDroneSpawnInterval = 1f;

    [Header("Boss1phase2")]
    private bool isWeakPoint1Disabled;
    private bool isWeakPoint2Disabled;
    private bool isWeakPoint1Instantiated;
    private bool isWeakPoint2Instantiated;
    public float phase2TransitionDuration = 10f;
    private float remainingPhase2TransitionTime;
    private float weakPoint1DamageTaken;
    private float weakPoint2DamageTaken;
    public GameObject weakPointPrefab1;
    public GameObject weakPointPrefab2;
    private GameObject currentWeakPoint1;
    private GameObject currentWeakPoint2;
    public Transform weakPointSpawn1;
    public Transform weakPointSpawn2;

    [Header("Boss1openprotected & Boss1phase2")]
    public float droneSpawnInterval = 1f;
    public Transform droneSpawnPoint1;
    public Transform droneSpawnPoint2;

    [Header("Boss1phase2 Lasers")]
    public float laserFiringDelay = 3f;
    public float trackingDelay = 0.5f;
    public float lasersDamagePerSecond = 5f;
    private float currentLaserFiringDelay;
    private float currentLaserFiringDelayMax = 3f;
    public GameObject laserParticlePrefab;
    public GameObject additionalParticlePrefab;
    private GameObject currentLaserParticle1;
    private GameObject currentLaserParticle2;
    private GameObject currentAdditionalParticle1;
    private GameObject currentAdditionalParticle2;
    public Transform laserSpawnPoint1;
    public Transform laserSpawnPoint2;
    public Transform additionalParticleSpawnPoint1;
    public Transform additionalParticleSpawnPoint2;
    public LayerMask lasersHitLayers;
    private Vector2 targetDirection1;
    private Vector2 targetDirection2;

    [Header("Death Stuff")]
    public GameObject bossExplosionPrefab;
    public GameObject screenWipePrefab;
    public Transform bossExplosionPrefabSpot;

    [Header("Damage Feedback")]
    public GameObject damageFeedbackPrefab;
    private GameObject weakPoint1ExplosionInstance;
    private GameObject weakPoint2ExplosionInstance;
    public GameObject weakPointExplodePrefab;
    public Transform weakPoint1ExplosionSpot;
    public Transform weakPoint2ExplosionSpot;

    void Start()
    {
        animator = GetComponent<Animator>();
        phase1DroneSpawnTimer = phase1DroneSpawnInterval;
        rb = GetComponent<Rigidbody2D>();
        currentLaserFiringDelay = laserFiringDelay;
        SetPhase(BossPhase.Spawn);
        phase1DroneSpawnTimer = droneSpawnInterval;
    }

    void Update()
    {
        switch (currentPhase)
        {
            case BossPhase.Boss1phase1:
                Phase1Update();
                chargeUpTimer -= Time.deltaTime;
                phase1DroneSpawnTimer -= Time.deltaTime;
                if (phase1DroneSpawnTimer <= 0f)
                {
                    //SpawnDrone(droneSpawnPointPhase1);
                    phase1DroneSpawnTimer = droneSpawnInterval;
                }
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
                    StartCoroutine(TransitionToBoss1open());
                }
                break;

            case BossPhase.Boss1open:
                break;

            case BossPhase.Boss1openprotected:
                protectedDuration -= Time.deltaTime;
                phase1DroneSpawnTimer -= Time.deltaTime;
                if (phase1DroneSpawnTimer <= 0f)
                {
                    SpawnDrone(droneSpawnPoint1);
                    SpawnDrone(droneSpawnPoint2);
                    phase1DroneSpawnTimer = droneSpawnInterval;
                }
                if (protectedDuration <= 0f)
                {
                    SetPhase(BossPhase.Boss1phase2);
                }
                break;

            case BossPhase.Boss1phase2:
                remainingPhase2TransitionTime -= Time.deltaTime;
                if (remainingPhase2TransitionTime <= 0f)
                {
                    SetPhase(BossPhase.Boss1openprotected);
                    DeactivateWeakPoints();
                    if (currentLaserParticle1 != null)
                    {
                        Destroy(currentLaserParticle1);
                        currentLaserParticle1 = null;
                    }
                    if (currentLaserParticle2 != null)
                    {
                        Destroy(currentLaserParticle2);
                        currentLaserParticle2 = null;
                    }
                }
                else
                {
                    Phase2Update();
                    Phase2Cleanup();
                    ShootLasersAtPlayer();
                    if (!enteredPhase2)
                    {
                        currentLaserFiringDelay = laserFiringDelay;
                        enteredPhase2 = true;
                    }
                    if (currentAdditionalParticle1 == null && isWeakPoint2Disabled == false)
                    {
                        currentAdditionalParticle1 = Instantiate(additionalParticlePrefab, additionalParticleSpawnPoint1.position, additionalParticleSpawnPoint1.rotation);
                    }
                    if (currentAdditionalParticle2 == null && isWeakPoint1Disabled == false)
                    {
                        currentAdditionalParticle2 = Instantiate(additionalParticlePrefab, additionalParticleSpawnPoint2.position, additionalParticleSpawnPoint2.rotation);
                    }
                    phase1DroneSpawnTimer -= Time.deltaTime;
                    if (phase1DroneSpawnTimer <= 0f)
                    {
                        SpawnDrone(droneSpawnPoint1);
                        SpawnDrone(droneSpawnPoint2);
                        phase1DroneSpawnTimer = droneSpawnInterval;
                    }
                }
                break;

            case BossPhase.Death:
                break;

            case BossPhase.Destruction:
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
            SpawnDrone(droneSpawnPointPhase1);
            phase1DroneSpawnTimer = phase1DroneSpawnInterval;
        }
    }

    private void Phase2Update()
    {
        phase1DroneSpawnTimer -= Time.deltaTime;
        if (phase1DroneSpawnTimer <= 0f)
        {
            SpawnDrone(droneSpawnPoint1);
            SpawnDrone(droneSpawnPoint2);
            phase1DroneSpawnTimer = droneSpawnInterval;
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

    void SpawnDrone(Transform spawnPoint)
    {
        GameObject newDrone = Instantiate(bossDronePrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private void StartChargeUp()
    {
        currentChargeUp = Instantiate(chargeUpPrefab, chargeUpSpawnPoint.position, Quaternion.identity, transform);
        AudioManager.instance.Play("chargeup");
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
            yield return new WaitForSeconds(5);
            StopFiringLaser();
        }
    }

    private void FireLaser()
    {
        isFiringLaser = true;
        laserLineRenderer.enabled = true;

        Debug.Log("Firing laser...");

        if (currentBossBeam == null)
        {
            Debug.Log("Instantiating boss beam...");
            currentBossBeam = Instantiate(bossBeamPrefab, chargeUpSpawnPoint.position, Quaternion.identity);
            AudioManager.instance.Play("bwaaa");
        }
        else
        {
            Debug.Log("Moving boss beam...");
            currentBossBeam.transform.position = chargeUpSpawnPoint.position;
        }

        RaycastHit2D hit = Physics2D.Raycast(chargeUpSpawnPoint.position, Vector2.left, Mathf.Infinity, laserHitLayers);

        if (hit.collider != null)
        {
            Debug.Log("Laser hit object: " + hit.collider.name);

            laserHitDistance = hit.distance;
            laserLineRenderer.SetPosition(1, new Vector3(-laserHitDistance, 0, 0));

            if (hit.collider.CompareTag("PlayerShip"))
            {
                Debug.Log("Player ship hit!");

                ShieldDamage shieldDamage = hit.collider.GetComponent<ShieldDamage>();

                if (shieldDamage != null)
                {
                    Debug.Log("Dealing damage to player shield...");
                    shieldDamage.TakeDamage(laserDamagePerSecond * Time.deltaTime);
                }
            }
        }
        else
        {
            Debug.Log("Laser did not hit any objects.");
            laserLineRenderer.SetPosition(1, new Vector3(-1000, 0, 0));
        }
    }


    public void StopFiringLaser()
    {
        isFiringLaser = false;
        laserLineRenderer.enabled = false;
        AudioManager.instance.Stop("bwaaa");
        if (currentBossBeam != null)
        {
            Destroy(currentBossBeam);
            currentBossBeam = null;
        }
        if (currentChargeUp != null)
        {
            Destroy(currentChargeUp);
            currentChargeUp = null;
            chargeUpTimer = 5f;
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
        Instantiate(damageFeedbackPrefab, chargeUpFeedbackPoint.position, Quaternion.identity);
        if (chargeUpDamageTaken >= chargeUpDamageThreshold)
        {
            chargeUpDestroyedCount++;
            if (chargeUpDestroyedCount == 5)
            {
                SetPhase(BossPhase.Boss1phase1end);
            }
        }
    }

    public void MySecretShame()
    {
        Instantiate(damageFeedbackPrefab, chargeUpFeedbackPoint.position, Quaternion.identity);
    }

    private IEnumerator OpenAnimation()
    {
        yield return new WaitForSeconds(openAnimationTime);
        SetPhase(BossPhase.Boss1openprotected);
    }

    public void OnSpawnAnimationEnd()
    {
        SetPhase(BossPhase.Boss1phase1);
    }

    IEnumerator TransitionToBoss1phase1()
    {
        yield return new WaitForSeconds(5.5f);
    }

    IEnumerator TransitionToBoss1open()
    {
        float centerY = -1.25f;
        Vector2 targetPosition = new Vector2(transform.position.x, centerY);
        float acceleration = 5f;
        float maxSpeed = 2f;
        while (!Mathf.Approximately(transform.position.y, centerY))
        {
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
        yield return new WaitForSeconds(phase1EndTransitionTime);
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
        if (!isWeakPoint1Instantiated && isWeakPoint1Disabled == false)
        {
            currentWeakPoint1 = Instantiate(weakPointPrefab1, weakPointSpawn1.position, Quaternion.identity, transform);
            isWeakPoint1Instantiated = true;
        }
        if (!isWeakPoint2Instantiated && isWeakPoint2Disabled == false)
        {
            currentWeakPoint2 = Instantiate(weakPointPrefab2, weakPointSpawn2.position, Quaternion.identity, transform);
            isWeakPoint2Instantiated = true;
        }
    }

    private void DeactivateWeakPoints()
    {
        if (currentWeakPoint1 != null)
        {
            Destroy(currentWeakPoint1);
            Destroy(currentAdditionalParticle2);
            currentWeakPoint1 = null;
            isWeakPoint1Instantiated = false;
        }
        if (currentWeakPoint2 != null)
        {
            Destroy(currentWeakPoint2);
            Destroy(currentAdditionalParticle1);
            currentWeakPoint2 = null;
            isWeakPoint2Instantiated = false;
        }
    }

    private void Phase2Cleanup()
    {
        if (isWeakPoint1Disabled == true)
        {
            Destroy(currentWeakPoint1);
            Destroy(currentAdditionalParticle2);
            Destroy(currentLaserParticle1);
            currentLaserParticle1 = null;
            currentWeakPoint1 = null;
            isWeakPoint1Instantiated = false;
        }
        if (isWeakPoint2Disabled == true)
        {
            Destroy(currentWeakPoint2);
            Destroy(currentAdditionalParticle1);
            Destroy(currentLaserParticle2);
            currentLaserParticle2 = null;
            currentWeakPoint2 = null;
            isWeakPoint2Instantiated = false;
        }
    }


    public void WeakPointTakeDamage(int weakPointNumber, float damage)
    {
        if (weakPointNumber == 1)
        {
            weakPoint1DamageTaken += damage;
            Instantiate(damageFeedbackPrefab, weakPointSpawn1.position, Quaternion.identity);
            if (weakPoint1DamageTaken >= 500 && !isWeakPoint1Disabled)
            {
                isWeakPoint1Disabled = true;
                weakPoint1ExplosionInstance = Instantiate(weakPointExplodePrefab, weakPoint1ExplosionSpot.position, Quaternion.identity);
                AudioManager.instance.Play("bossinterrupt");
            }
        }
        else if (weakPointNumber == 2)
        {
            weakPoint2DamageTaken += damage;
            Instantiate(damageFeedbackPrefab, weakPointSpawn2.position, Quaternion.identity);
            if (weakPoint2DamageTaken >= 500 && !isWeakPoint2Disabled)
            {
                isWeakPoint2Disabled = true;
                weakPoint2ExplosionInstance = Instantiate(weakPointExplodePrefab, weakPoint2ExplosionSpot.position, Quaternion.identity);
                AudioManager.instance.Play("bossinterrupt");


            }
        }
        if (isWeakPoint1Disabled && isWeakPoint2Disabled)
        {
            SetPhase(BossPhase.Death);
        }
    }



    private void ShootLasersAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("PlayerShip");
        if (player != null)
        {
            Vector2 newDirection1 = (player.transform.position - laserSpawnPoint1.position).normalized;
            Vector2 newDirection2 = (player.transform.position - laserSpawnPoint2.position).normalized;
            targetDirection1 = Vector2.Lerp(targetDirection1, newDirection1, Time.deltaTime / trackingDelay);
            targetDirection2 = Vector2.Lerp(targetDirection2, newDirection2, Time.deltaTime / trackingDelay);
            Quaternion rotation1 = Quaternion.LookRotation(Vector3.forward, targetDirection1);
            Quaternion rotation2 = Quaternion.LookRotation(Vector3.forward, targetDirection2);
            if (currentLaserParticle1 != null && isWeakPoint1Disabled == false)
            {
                RaycastHit2D hit1 = Physics2D.Raycast(laserSpawnPoint1.position, targetDirection1, Mathf.Infinity, lasersHitLayers);
                if (hit1.collider != null && hit1.collider.CompareTag("PlayerShip"))
                {
                    ShieldDamage shieldDamage = hit1.collider.GetComponent<ShieldDamage>();
                    if (shieldDamage != null)
                    {
                        shieldDamage.TakeDamage(lasersDamagePerSecond * Time.deltaTime);
                    }
                }
            }
            if (currentLaserParticle2 != null && isWeakPoint2Disabled == false)
            {
                RaycastHit2D hit2 = Physics2D.Raycast(laserSpawnPoint2.position, targetDirection2, Mathf.Infinity, lasersHitLayers);

                if (hit2.collider != null && hit2.collider.CompareTag("PlayerShip"))
                {
                    ShieldDamage shieldDamage = hit2.collider.GetComponent<ShieldDamage>();
                    if (shieldDamage != null)
                    {
                        shieldDamage.TakeDamage(lasersDamagePerSecond * Time.deltaTime);
                    }
                }
            }
            if (currentLaserParticle1 != null && isWeakPoint1Disabled == false)
            {
                currentLaserParticle1.transform.position = laserSpawnPoint1.position;
                currentLaserParticle1.transform.rotation = rotation1;
            }
            if (currentLaserParticle2 != null && isWeakPoint2Disabled == false)
            {
                currentLaserParticle2.transform.position = laserSpawnPoint2.position;
                currentLaserParticle2.transform.rotation = rotation2;
            }
            if (currentLaserFiringDelay > 0f)
            {
                currentLaserFiringDelay -= Time.deltaTime;
            }
            else
            {
                if (currentLaserParticle1 == null && isWeakPoint1Disabled == false)
                {
                    currentLaserParticle1 = Instantiate(laserParticlePrefab, laserSpawnPoint1.position, rotation1);
                }
                if (currentLaserParticle2 == null && isWeakPoint2Disabled == false)
                {
                    currentLaserParticle2 = Instantiate(laserParticlePrefab, laserSpawnPoint2.position, rotation2);
                }
            }
        }
    }

    private void CleanUpAll()
    {
        Destroy(currentWeakPoint1);
        Destroy(currentAdditionalParticle2);
        Destroy(currentLaserParticle1);
        currentLaserParticle1 = null;
        currentWeakPoint1 = null;
        isWeakPoint1Instantiated = false;
        Destroy(currentWeakPoint2);
        Destroy(currentAdditionalParticle1);
        Destroy(currentLaserParticle2);
        currentLaserParticle2 = null;
        currentWeakPoint2 = null;
        isWeakPoint2Instantiated = false;
    }

    IEnumerator ShutItDown()
    {
        yield return new WaitForSeconds(4f);
        SetPhase(BossPhase.Destruction);
        if (weakPoint1ExplosionInstance != null)
        {
            Destroy(weakPoint1ExplosionInstance);
        }
        if (weakPoint2ExplosionInstance != null)
        {
            Destroy(weakPoint2ExplosionInstance);
        }
    }

    IEnumerator FinalPurge()
    {
        yield return new WaitForSeconds(.75f);
        Destroy(gameObject);
        GameManager.Instance.isGameOver = true;
        GameManager.Instance.YouWin();
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
                animator.enabled = false;
                SetNewTargetPosition();
                break;

            case BossPhase.Boss1phase1end:
                break;

            case BossPhase.Boss1open:
                animator.enabled = true;
                animator.SetTrigger("Boss1open");
                StartCoroutine(OpenAnimation());
                break;

            case BossPhase.Boss1openprotected:
                currentLaserFiringDelay = currentLaserFiringDelayMax;
                break;

            case BossPhase.Boss1phase2:
                protectedDuration = protectedDurationMax;
                remainingPhase2TransitionTime = phase2TransitionDuration;
                ActivateWeakPoints();
                break;

            case BossPhase.Death:
                animator.SetTrigger("Death");
                Instantiate(bossExplosionPrefab, bossExplosionPrefabSpot.position, Quaternion.identity);
                CleanUpAll();
                StartCoroutine(ShutItDown());
                break;

            case BossPhase.Destruction:
                Instantiate(screenWipePrefab, bossExplosionPrefabSpot.position, Quaternion.identity);
                StartCoroutine(FinalPurge());
                break;
        }
    }
}