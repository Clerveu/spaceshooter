using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy1Attack : MonoBehaviour
{
    public GameObject enemy1AttackPrefab;
    public GameObject projectilePrefab;
    public float spawnInterval = 5f;
    public float initialDelay = 10f;

    void Start()
    {
        StartCoroutine(SpawnAttacks());
    }

    IEnumerator SpawnAttacks()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            GameObject playerShip = GameObject.FindGameObjectWithTag("PlayerShip");

            if (playerShip == null)
            {
                // PlayerShip doesn't exist, stop the coroutine
                yield break;
            }

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Enemy1Attack enemy1Attack = projectile.GetComponent<Enemy1Attack>();
            enemy1Attack.target = playerShip.transform;
            enemy1Attack.damageAmount = 10f;
            enemy1Attack.projectileSpeed = 5f;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
