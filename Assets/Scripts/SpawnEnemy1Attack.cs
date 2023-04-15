using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy1Attack : MonoBehaviour
{
    public GameObject enemy1AttackPrefab;
    public GameObject projectilePrefab;
    public float spawnInterval = 5f;
    public float initialDelay = 10f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAttacks());
    }

    IEnumerator SpawnAttacks()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Enemy1Attack enemy1Attack = projectile.GetComponent<Enemy1Attack>();
            enemy1Attack.target = GameObject.FindGameObjectWithTag("PlayerShip").transform;
            enemy1Attack.damageAmount = 10f;
            enemy1Attack.projectileSpeed = 5f;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
