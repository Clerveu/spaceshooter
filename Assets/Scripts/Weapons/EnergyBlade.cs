using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBlade : MonoBehaviour
{
    public GameObject energyBladePrefab;
    public AnimationCurve rotationSpeedCurve;
    public AnimationCurve scaleCurve;
    public float damageAmount = 10f;
    public float lifetime = 5f; // lifetime of the blade in seconds

    private GameObject energyBladeInstance;
    private float elapsedTime;
    private float currentScale;

    private void Start()
    {
      
    }

    public void StartEnergyBlade()
    {
        if (energyBladeInstance == null)
        {
            energyBladeInstance = Instantiate(energyBladePrefab, transform.position, Quaternion.identity);
            energyBladeInstance.transform.SetParent(transform);
            energyBladeInstance.transform.localScale = Vector3.zero;
            elapsedTime = 0f;
            currentScale = 0f;
        }
    }

    private void Update()
    {
        if (energyBladeInstance != null)
        {
            elapsedTime += Time.deltaTime;
            float timeNormalized = elapsedTime / lifetime;

            // Scale the blade
            currentScale = scaleCurve.Evaluate(timeNormalized);
            energyBladeInstance.transform.localScale = Vector3.one * currentScale;

            // Rotate the blade
            float currentRotationSpeed = rotationSpeedCurve.Evaluate(timeNormalized);
            energyBladeInstance.transform.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);

            // Destroy the blade at the end of its lifetime
            if (elapsedTime >= lifetime)
            {
                Destroy(energyBladeInstance);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            other.GetComponent<ShieldDamage>().TakeDamage(damageAmount);
        }
    }
}
