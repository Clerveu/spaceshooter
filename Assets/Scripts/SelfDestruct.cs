using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float lifetime = 20f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
