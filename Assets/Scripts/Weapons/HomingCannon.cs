using UnityEngine;

public class HomingCannon : MonoBehaviour
{
    public float damageAmount = 10f;
    public float projectileSpeed = 5f;
    public float homingRange = 100f;
    public float rotationSpeed = 200f;

    private float maxDistance = 10f;
    private float traveledDistance = 0f;
    private GameObject target;

    void Start()
    {
        target = FindClosestEnemy();
    }

    void Update()
    {
        target = FindClosestEnemy();

        // Homing behavior
        if (target == null)
        {
            target = FindClosestEnemy();
            return;
        }

        Vector2 direction = (Vector2)target.transform.position - (Vector2)transform.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        GetComponent<Rigidbody2D>().angularVelocity = -rotateAmount * rotationSpeed;

        GetComponent<Rigidbody2D>().velocity = transform.up * projectileSpeed;

        if (Vector2.Distance(transform.position, target.transform.position) < homingRange)
        {
            target = FindClosestEnemy();
        }

        // Projectile movement
        transform.Translate(Vector3.right * projectileSpeed * Time.deltaTime);

        // Keep track of the distance traveled
        traveledDistance += projectileSpeed * Time.deltaTime;

        // Check if the projectile has gone beyond the maximum distance
        if (traveledDistance >= maxDistance)
        {
            Destroy(gameObject); // Destroy the projectile
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().TakeDamage(damageAmount);
            Destroy(gameObject); // Destroy the projectile
        }
    }
}
