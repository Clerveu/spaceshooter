using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool shaking = false;

    public float shakeDuration = 0.3f;
    public float shakeIntensity = 0.1f;

    public void ShakeCamera(float intensity)
    {
        if (!shaking && GameManager.Instance.enableScreenShake)
        {
            originalPosition = transform.localPosition;
            StartCoroutine(Shake(intensity));
        }
    }

    private IEnumerator Shake(float intensity)
    {
        shaking = true;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        shaking = false;
        transform.localPosition = originalPosition;
    }
}
