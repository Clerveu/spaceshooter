using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearX : MonoBehaviour
{
    public float speed = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
    }
}
