using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Level1Checkpoint : MonoBehaviour
{
    public static Level1Checkpoint Instance;
    public GameObject Spawn1;
    public GameObject Spawn2;
    public GameObject BossSpawn;
    public GameObject CheckpointSpawn;
    public GameObject Jukebox;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogError("Another instance of Level1Checkpoint already exists!");
            Destroy(this);
        }
    }

    public void CheckpointActive()
    {
        Debug.Log("CheckpoingActive doin' its thing!");
        Spawn1.SetActive(false);
        Spawn2.SetActive(false);
        BossSpawn.SetActive(false);
        Jukebox.SetActive(false);
        CheckpointSpawn.SetActive(true);
    }
}