using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CheckpointQuery : MonoBehaviour
{
    PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        QueryCheckpoint();
    }

    private void QueryCheckpoint()
    {
        if (CheckpointManager.Instance.pastCheckpoint)
        {
            Level1Checkpoint.Instance.CheckpointActive();
            if (playerController != null && CheckpointManager.Instance.collectedSpecialWeapons != null)
            {
                foreach (PlayerController.SpecialWeapon weapon in CheckpointManager.Instance.collectedSpecialWeapons)
                {
                    playerController.AddSpecialWeapon(weapon);
                }
            }
        }
    }
}
