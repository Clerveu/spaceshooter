using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBack3 : MonoBehaviour
{
    public float scrollSpeed = 20f;

    private Transform Layer3;
    private Transform Layer3dup;
    private float backgroundWidth;

    private void Start()
    {
        Layer3 = transform.Find("Layer3");
        Layer3dup = transform.Find("Layer3dup");
        backgroundWidth = Layer3.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, backgroundWidth);
        Layer3.position = new Vector3(-newPosition, Layer3.position.y, Layer3.position.z);
        Layer3dup.position = new Vector3(Layer3.position.x + backgroundWidth, Layer3dup.position.y, Layer3dup.position.z);

        if (Layer3dup.position.x < 0)
        {
            Layer3.position = new Vector3(Layer3dup.position.x + backgroundWidth, Layer3.position.y, Layer3.position.z);
            Transform temp = Layer3;
            Layer3 = Layer3dup;
            Layer3dup = temp;
        }
    }

}


