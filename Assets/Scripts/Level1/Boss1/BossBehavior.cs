using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public enum BossPhase { Spawn, Boss1phase1, Boss1phase1end, Boss1open, Boss1openprotected, Boss1phase2, Death }

    private Animator animator;
    private bool isCharging;
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

    [Header("Laser")]
    public LineRenderer laserLineRenderer;
    public float laserDamagePerSecond = 5f;
    public LayerMask laserHitLayers;
    private bool isFiringLaser;
    private float laserHitDistance;
    public GameObject bossBeamPrefab;
    private GameObject currentBossBeam;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        phase1DroneSpawnTimer = phase1DroneSpawnInterval;
        rb = GetComponent<Rigidbody2D>(); // Initialize the Rigidbody2D variable
        SetPhase(BossPhase.Spawn);
    }

    // Update is called once per frame
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
        }
    }


    void FixedUpdate()
    {
        if (currentPhase == BossPhase.Boss1phase1)
        {
            MoveToTargetPosition();
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
                SetNewTargetPosition();
                break;
        }
    }

    IEnumerator TransitionToBoss1phase1()
    {
        yield return new WaitForSeconds(5f); // Delay before transitioning to Boss1phase1
        SetPhase(BossPhase.Boss1phase1);
    }

    private void Phase1Update()
    {
        phase1DroneSpawnTimer -= Time.deltaTime;
        if (phase1DroneSpawnTimer <= 0f)
        {
            SpawnEnemy1Attack();
            phase1DroneSpawnTimer = phase1DroneSpawnInterval;
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

        // Calculate the direction and distance to the target position
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetPosition);

        // Apply acceleration towards the target position
        rb.AddForce(direction * acceleration, ForceMode2D.Force);

        // Limit the maximum speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);

        // If the boss is close to the target position, set a new target position
        if (distance < 0.1f && !isFiringLaser)
        {
            SetNewTargetPosition();
        }
    }

    private void SpawnEnemy1Attack()
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

        if (chargeUpDamageTaken < chargeUpDamageThreshold)
        {
            FireLaser();
            yield return new WaitForSeconds(5); // Add a delay before stopping the laser
            StopFiringLaser();
        }
        else
        {
            // Apply damage to the boss
        }

        StopFiringLaser();
        Destroy(currentChargeUp);
        currentChargeUp = null;
    }


    private void FireLaser()
    {
        isFiringLaser = true;
        laserLineRenderer.enabled = true;

        // Instantiate the BossBeam prefab only if it doesn't exist
        if (currentBossBeam == null)
        {
            currentBossBeam = Instantiate(bossBeamPrefab, chargeUpSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            // Update the position of the existing BossBeam prefab
            currentBossBeam.transform.position = chargeUpSpawnPoint.position;
        }

        // Change the direction of the raycast to Vector2.left to fire towards the left
        RaycastHit2D hit = Physics2D.Raycast(chargeUpSpawnPoint.position, Vector2.left, Mathf.Infinity, laserHitLayers);
        if (hit.collider != null)
        {
            laserHitDistance = hit.distance;

            // Change to negative laserHitDistance to position the laser correctly
            laserLineRenderer.SetPosition(1, new Vector3(-laserHitDistance, 0, 0));

            if (hit.collider.CompareTag("PlayerShip"))
            {
                // Apply damage to the player
                ShieldDamage shieldDamage = hit.collider.GetComponent<ShieldDamage>();
                if (shieldDamage != null)
                {
                    shieldDamage.TakeDamage(laserDamagePerSecond * Time.deltaTime);
                }
            }
        }
        else
        {
            // Change to negative value to position the laser correctly when it doesn't hit anything
            laserLineRenderer.SetPosition(1, new Vector3(-1000, 0, 0));
        }
    }


    private void StopFiringLaser()
    {
        isFiringLaser = false;
        laserLineRenderer.enabled = false;
        
        if (currentBossBeam != null)
        {
            Destroy(currentBossBeam);
            currentBossBeam = null;
        }
    }

    public void ChargeUpTakeDamage(float damage)
    {
        chargeUpDamageTaken += damage;
    }
}