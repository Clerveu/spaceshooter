using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertSine : MonoBehaviour
{
    public float speed = 5f;
    public float amplitude = 2f;
    public float frequency = 0.5f;

    private float startY;
    private float timeElapsed;

    void Start()
    {
        startY = transform.position.y;
        StartCoroutine(UpdatePosition());
    }

    IEnumerator UpdatePosition()
    {
        while (true)
        {
            timeElapsed += Time.deltaTime;
            float y = startY + amplitude * Mathf.Sin(frequency * timeElapsed * speed);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            yield return null;
        }
    }
}
