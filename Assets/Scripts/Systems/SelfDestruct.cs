using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float lifetime = 20f;
    public bool selfDestructed = false;

    private void Start()
    {
        StartCoroutine(SelfDestructCoroutine());
    }

    IEnumerator SelfDestructCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        selfDestructed = true;
        Destroy(gameObject);
    }

    public bool SelfDestructed
    {
        get { return selfDestructed; }
    }
}
