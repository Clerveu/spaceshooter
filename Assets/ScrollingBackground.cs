using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgroundBackup : MonoBehaviour
{
    public float scrollSpeed = 20f;

    private Transform background1;
    private Transform background2;
    private float backgroundWidth;

    private void Start()
    {
        background1 = transform.Find("background1");
        background2 = transform.Find("background2");
        backgroundWidth = background1.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, backgroundWidth);
        background1.position = Vector3.left * newPosition;
        background2.position = (Vector3)background1.position + Vector3.right * backgroundWidth;

        if (background2.position.x < 0)
        {
            background1.position = (Vector3)background2.position + Vector3.right * backgroundWidth;
            Transform temp = background1;
            background1 = background2;
            background2 = temp;
        }
    }
}


