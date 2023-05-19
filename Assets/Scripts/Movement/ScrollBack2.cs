using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBack2 : MonoBehaviour
{
    public float scrollSpeed = 2f;

    private Transform Layer2;
    private Transform Layer2dup;
    private float backgroundWidth;

    private void Start()
    {
        Layer2 = transform.Find("Layer2");
        Layer2dup = transform.Find("Layer2dup");
        backgroundWidth = Layer2.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, backgroundWidth);
        Layer2.position = new Vector3(-newPosition + 0.4870899f, Layer2.position.y, Layer2.position.z);
        Layer2dup.position = new Vector3(Layer2.position.x + backgroundWidth, Layer2dup.position.y, Layer2dup.position.z);


        if (Layer2dup.position.x < 0)
        {
            Layer2.position = new Vector3(Layer2dup.position.x + backgroundWidth, Layer2.position.y, Layer2.position.z);
            Transform temp = Layer2;
            Layer2 = Layer2dup;
            Layer2dup = temp;
        }
    }

}

