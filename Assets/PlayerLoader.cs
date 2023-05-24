using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public GameObject playerPrefab;

    void Awake()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
    }
}
