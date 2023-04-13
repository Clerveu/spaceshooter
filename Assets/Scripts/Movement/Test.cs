using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed = 1f;
    public float rotationSpeed = 200f;
    public float homingRange = 100f;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindClosestEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = FindClosestEnemy();
            return;
        }

        Vector2 direction = (Vector2)target.transform.position - (Vector2)transform.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        GetComponent<Rigidbody2D>().angularVelocity = -rotateAmount * rotationSpeed;

        GetComponent<Rigidbody2D>().velocity = transform.up * speed;

        if (Vector2.Distance(transform.position, target.transform.position) < homingRange)
        {
            target = FindClosestEnemy();
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = enemy;
                distance = curDistance;
            }
        }
        return closest;
    }
}
