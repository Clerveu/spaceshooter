using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBack4 : MonoBehaviour
{
    public float scrollSpeed = 20f;

    private Transform Layer4;
    private Transform Layer4dup;
    private float backgroundWidth;

    private void Start()
    {
        Layer4 = transform.Find("Layer4");
        Layer4dup = transform.Find("Layer4dup");
        backgroundWidth = Layer4.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, backgroundWidth);
        Layer4.position = Vector3.left * newPosition;
        Layer4dup.position = (Vector3)Layer4.position + Vector3.right * backgroundWidth;

        if (Layer4dup.position.x < 0)
        {
            Layer4.position = (Vector3)Layer4dup.position + Vector3.right * backgroundWidth;
            Transform temp = Layer4;
            Layer4 = Layer4dup;
            Layer4dup = temp;
        }
    }
}


