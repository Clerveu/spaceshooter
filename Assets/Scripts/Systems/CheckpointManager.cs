using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    public bool pastCheckpoint;
    public List<PlayerController.SpecialWeapon> collectedSpecialWeapons;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCheckpoint(bool pastCheckpoint, List<PlayerController.SpecialWeapon> collectedSpecialWeapons)
    {
        this.pastCheckpoint = pastCheckpoint;
        this.collectedSpecialWeapons = new List<PlayerController.SpecialWeapon>(collectedSpecialWeapons);
    }

    public void ResetCheckpoint()
    {
        pastCheckpoint = false;
        collectedSpecialWeapons.Clear();
    }
}
