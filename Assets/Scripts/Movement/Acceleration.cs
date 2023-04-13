using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceleration : MonoBehaviour
{
    public float initialSpeed = 0f;
    public float acceleration = 10f;
    private float currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed += acceleration * Time.deltaTime;
        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
    }
}