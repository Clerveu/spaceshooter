using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle : MonoBehaviour
{
    public float initialAngle = 0f; // angle in degrees

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, initialAngle);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
