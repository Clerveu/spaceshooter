using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingDrone : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float healAmount = 20f;
    public float healDuration = 5f;
    private float healStartTime;
    public GameObject healEffectPrefab;
    public GameObject healEffect;
    public Transform healEffectTransform;
    private GameObject playerShip;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
        healStartTime = Time.time;
        healEffect = Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
        healEffectTransform = healEffect.transform;
        health = playerShip.GetComponent<Health>();
        if (health != null)
        {
            StartCoroutine(RestoreHealthOverTime());
        }
    }

    public void InitializeDrone(GameObject playerShipReference)
    {
        playerShip = playerShipReference;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 circlePosition = new Vector2(Mathf.Sin(Time.time * rotationSpeed), Mathf.Cos(Time.time * rotationSpeed));
        Vector2 targetPosition = (Vector2)playerShip.transform.position + circlePosition * 1.5f;

        // Interpolate the drone's position with Lerp
        float lerpFactor = 0.2f; // You can adjust this value to change the detachment effect
        transform.position = Vector2.Lerp(transform.position, targetPosition, lerpFactor);

        Vector2 direction = playerShip.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Calculate the end position of the healing beam
        float distanceToPlayer = direction.magnitude;
        Vector2 beamEndPosition = (Vector2)transform.position + direction.normalized * distanceToPlayer;

        // Set the position of the secondary particle system to the end position of the healing beam
        healEffectTransform.position = beamEndPosition;

        // Set the rotation of the heal effect to match the player ship's rotation
        healEffectTransform.rotation = playerShip.transform.rotation;

        // If the healing duration is over, destroy the drone
        if (Time.time - healStartTime > healDuration)
        {
            AudioManager.instance.Stop("healingdrone");
            Destroy(healEffect);
            Destroy(gameObject);
        }
    }

    private IEnumerator RestoreHealthOverTime()
    {
        float elapsedTime = 0f;
        float healPerSecond = healAmount / healDuration;

        while (elapsedTime < healDuration)
        {
            health.currentHealth += healPerSecond * Time.deltaTime;
            if (health.currentHealth > health.maxHealth)
            {
                health.currentHealth = health.maxHealth;
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
