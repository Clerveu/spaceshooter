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
        Layer2.position = Vector3.left * newPosition;
        Layer2dup.position = (Vector3)Layer2.position + Vector3.right * backgroundWidth;

        if (Layer2dup.position.x < 0)
        {
            Layer2.position = (Vector3)Layer2dup.position + Vector3.right * backgroundWidth;
            Transform temp = Layer2;
            Layer2 = Layer2dup;
            Layer2dup = temp;
        }
    }
}

