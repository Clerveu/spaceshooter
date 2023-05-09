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
        yield return new WaitForSeconds(spawnTime);
        Instantiate(warpEffectPrefab, transform.position, Quaternion.identity);
        StartCoroutine(SpawnBoss());
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(bossPrefab, transform.position, Quaternion.identity);
    }
}