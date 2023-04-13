using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertSine : MonoBehaviour
{
    public float speed = 5f;
    public float amplitude = 2f;
    public float frequency = 0.5f;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float y = startY + amplitude * Mathf.Sin(frequency * Time.time * speed);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
