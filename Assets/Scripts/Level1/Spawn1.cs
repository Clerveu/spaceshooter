using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn1 : MonoBehaviour
{
    public GameObject enemy1Prefab;
    public AnimationCurve spawnIntervalCurve = AnimationCurve.Linear(0f, 5f, 120f, 5f);
    public float spawnStartTime = 10f; // time at which spawning will start
    public float spawnDuration = 120f; // duration for which enemies will spawn

    private float timeElapsed = 0f;
    [SerializeField]
    private float yPositionVariance = 0.5f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Check if timeElapsed has exceeded the spawnStartTime, if not, skip this iteration
            if (timeElapsed < spawnStartTime)
            {
                yield return null;
                continue;
            }

            // Check if timeElapsed has exceeded the spawnDuration, if so, stop the coroutine
            if (timeElapsed >= spawnDuration + spawnStartTime)
            {
                break;
            }

            float modifiedSpawnInterval = spawnIntervalCurve.Evaluate(timeElapsed - spawnStartTime);
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += Random.Range(-yPositionVariance, yPositionVariance); // add a random y offset based on the yPositionVariance variable
            Instantiate(enemy1Prefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(modifiedSpawnInterval);
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
    }
}