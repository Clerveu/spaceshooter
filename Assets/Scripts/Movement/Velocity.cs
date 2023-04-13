using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    public float speed = 1f;
    public float angle = 0f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction based on the angle
        Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;

        // Move in the direction at the specified speed
        transform.Translate(direction * speed * Time.deltaTime);
    }
}