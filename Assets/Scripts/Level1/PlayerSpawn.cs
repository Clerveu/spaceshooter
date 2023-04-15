using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject playerShipPrefab;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerShipPrefab, transform.position, Quaternion.identity);
        playerShipPrefab.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
