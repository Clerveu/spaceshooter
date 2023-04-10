using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBack1 : MonoBehaviour
{
    public float scrollSpeed = 20f;

    private Transform Layer1;
    private Transform Layer1dup;
    private float backgroundWidth;

    private void Start()
    {
        Layer1 = transform.Find("Layer1");
        Layer1dup = transform.Find("Layer1dup");
        backgroundWidth = Layer1.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, backgroundWidth);
        Layer1.position = Vector3.left * newPosition;
        Layer1dup.position = (Vector3)Layer1.position + Vector3.right * backgroundWidth;

        if (Layer1dup.position.x < 0)
        {
            Layer1.position = (Vector3)Layer1dup.position + Vector3.right * backgroundWidth;
            Transform temp = Layer1;
            Layer1 = Layer1dup;
            Layer1dup = temp;
        }
    }
}


