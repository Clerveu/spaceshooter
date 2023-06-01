using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAnimation : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    [Header("Floats")]
    [SerializeField] private float xRange;
    [SerializeField] private float yRange;
    [SerializeField] private float instantiationInterval;
    [SerializeField] private float intervalRandomness;
    [SerializeField] private float scale;
    [SerializeField] private float scaleRandomness;

    [Header("Curves")]
    [SerializeField] private bool useCurves;
    [SerializeField] private AnimationCurve xRangeCurve;
    [SerializeField] private AnimationCurve yRangeCurve;
    [SerializeField] private AnimationCurve instantiationIntervalCurve;
    [SerializeField] private AnimationCurve intervalRandomnessCurve;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private AnimationCurve scaleRandomnessCurve;

    private float instantiationTime;

    private void Start()
    {
        instantiationTime = Time.time;
        StartCoroutine(InstantiateExplosions());
    }

    private IEnumerator InstantiateExplosions()
    {
        while (true)
        {
            float elapsedTime = Time.time - instantiationTime;
            float randomInterval = useCurves ?
                Random.Range(-intervalRandomnessCurve.Evaluate(elapsedTime), intervalRandomnessCurve.Evaluate(elapsedTime)) :
                Random.Range(-intervalRandomness, intervalRandomness);
            float currentInstantiationInterval = useCurves ? instantiationIntervalCurve.Evaluate(elapsedTime) : instantiationInterval;
            yield return new WaitForSeconds(currentInstantiationInterval + randomInterval);

            float currentXRange = useCurves ? xRangeCurve.Evaluate(elapsedTime) : xRange;
            float currentYRange = useCurves ? yRangeCurve.Evaluate(elapsedTime) : yRange;
            Vector3 randomPosition = new Vector3(
                transform.position.x + Random.Range(-currentXRange, currentXRange),
                transform.position.y + Random.Range(-currentYRange, currentYRange),
                transform.position.z
            );

            GameObject explosion = Instantiate(explosionPrefab, randomPosition, Quaternion.identity);
            
            AudioSource audioSource = AudioManager.instance.Play("explode1");
            if (audioSource != null)
            {
                float originalPitch = audioSource.pitch;
                audioSource.pitch = Random.Range(0.4f, 1f);
            }
            
            float randomScale = useCurves ?
                Random.Range(-scaleRandomnessCurve.Evaluate(elapsedTime), scaleRandomnessCurve.Evaluate(elapsedTime)) :
                Random.Range(-scaleRandomness, scaleRandomness);
            float currentScale = useCurves ? scaleCurve.Evaluate(elapsedTime) : scale;
            explosion.transform.localScale *= (currentScale + randomScale);
        }
    }
}
