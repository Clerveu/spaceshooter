using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingToPlayer : MonoBehaviour
{
    public float projectileSpeed = 5f;
    public float homingRange = 100f; // The range within which the projectile will start homing towards a target
    public float rotationSpeed = 200f; // The speed at which the projectile will rotate towards the target
    public float randomOffsetRange = 0.1f; // The range of the random offset added to the direction vector
    private GameObject target;

    void Start()
    {
        target = FindPlayer();
    }

    void Update()
    {
        // Homing behavior
        if (target == null)
        {
            target = FindPlayer();
            return;
        }

        Vector2 direction = (Vector2)target.transform.position - (Vector2)transform.position;
        direction.Normalize();

        // Add a random offset to the direction vector for more organic movement
        Vector2 randomOffset = new Vector2(Random.Range(-randomOffsetRange, randomOffsetRange), Random.Range(-randomOffsetRange, randomOffsetRange));
        direction += randomOffset;

        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        GetComponent<Rigidbody2D>().angularVelocity = -rotateAmount * rotationSpeed;

        GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;

        if (Vector2.Distance(transform.position, target.transform.position) < homingRange)
        {
            target = FindPlayer();
        }
    }

    GameObject FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("PlayerShip");
        return player;
    }
}
