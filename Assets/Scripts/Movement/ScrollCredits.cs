using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCredits : MonoBehaviour
{
    public float scrollSpeed = 20f;
    public float timeBeforeDeceleration = 30f; // Time before deceleration starts
    public float decelerationTime = 2f; // Time over which deceleration happens
    public float finalSpeed = 1f; // Final speed after deceleration

    private Transform Layer1;
    private Transform Layer1dup;
    private float backgroundWidth;
    private float initialSpeed;
    private float decelerationStartTime = -1f;
    private float totalDistanceTravelled = 0f;

    private void Start()
    {
        Layer1 = transform.Find("Layer1");
        Layer1dup = transform.Find("Layer1dup");
        backgroundWidth = Layer1.GetComponent<SpriteRenderer>().bounds.size.x;
        initialSpeed = scrollSpeed;
    }

    private void Update()
    {
        if (decelerationStartTime < 0 && Time.time >= timeBeforeDeceleration)
        {
            decelerationStartTime = Time.time;
        }

        if (decelerationStartTime >= 0)
        {
            // Decrease speed from initial to final over the deceleration time
            scrollSpeed = Mathf.Lerp(initialSpeed, finalSpeed, (Time.time - decelerationStartTime) / decelerationTime);
        }

        totalDistanceTravelled += scrollSpeed * Time.deltaTime;
        float newPosition = Mathf.Repeat(totalDistanceTravelled, backgroundWidth);
        Layer1.position = new Vector3(-newPosition + 0.4870899f, Layer1.position.y, Layer1.position.z);
        Layer1dup.position = new Vector3(Layer1.position.x + backgroundWidth, Layer1dup.position.y, Layer1dup.position.z);

        if (Layer1dup.position.x < 0)
        {
            Layer1.position = new Vector3(Layer1dup.position.x + backgroundWidth, Layer1.position.y, Layer1.position.z);
            Transform temp = Layer1;
            Layer1 = Layer1dup;
            Layer1dup = temp;
        }
    }
}
