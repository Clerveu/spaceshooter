using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject warpEffectPrefab;
    public float spawnTime;

    void Start()
    {
        StartCoroutine(SpawnBossPortal());
    }

    IEnumerator SpawnBossPortal()
    {
        float targetTime = Time.time + spawnTime;
        yield return new WaitUntil(() => Time.time >= targetTime);
        PlayerController player = FindObjectOfType<PlayerController>();
        CheckpointManager.Instance.UpdateCheckpoint(true, player.GetCollectedWeapons());
        AudioManager.instance.PlayMusic("bossmusic", 0f);
        Instantiate(warpEffectPrefab, transform.position, Quaternion.identity);
        StartCoroutine(SpawnBoss());
    }


    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(bossPrefab, transform.position, Quaternion.identity);
    }
}