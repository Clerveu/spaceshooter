using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject autoCannonPrefab;
    public GameObject homingProjectilePrefab;
    public GameObject healingDronePrefab;
    public float autoCannonFireRate = 10f;
    public float homingProjectileFireRate = 100f;
    public float healingDroneFireRate = 1f;
    public float moveSpeed = 5f;
    private string fireButton = "Fire1";
    private string healingDroneButton = "Fire2";
    private string homingProjectileButton = "Fire3";

    private float nextAutoCannonFireTime;
    private float nextHomingProjectileFireTime;
    private float nextHealingDroneFireTime;

    private void FireAutoCannon()
    {
        if (Input.GetButton(fireButton) && Time.time > nextAutoCannonFireTime)
        {
            nextAutoCannonFireTime = Time.time + 1f / autoCannonFireRate;
            GameObject bullet = Instantiate(autoCannonPrefab, transform.position + transform.right * 0.5f, Quaternion.identity);
            if (bullet != null)
            {
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = transform.right * 5;
            }
        }
    }

    private void FireHomingProjectile()
    {
        if (Input.GetButton(homingProjectileButton) && Time.time > nextHomingProjectileFireTime)
        {
            nextHomingProjectileFireTime = Time.time + 1f / homingProjectileFireRate;
            GameObject homingProjectile = Instantiate(homingProjectilePrefab, transform.position + transform.right * 0.5f, Quaternion.identity);
            if (homingProjectile != null)
            {
                Rigidbody2D homingProjectileRigidbody = homingProjectile.GetComponent<Rigidbody2D>();
                homingProjectileRigidbody.velocity = transform.right * 1;
            }
        }
    }

    private void FireHealingDrone()
    {
        if (Input.GetButton(healingDroneButton) && Time.time > nextHealingDroneFireTime)
        {
            nextHealingDroneFireTime = Time.time + 1f / healingDroneFireRate;
            GameObject healingDrone = Instantiate(healingDronePrefab, transform.position + transform.right * 0.5f, Quaternion.identity);
            HealingDrone healingDroneComponent = healingDrone.GetComponent<HealingDrone>();
            if (healingDroneComponent != null)
            {
                healingDroneComponent.InitializeDrone(gameObject);
            }
        }
    }




// Start is called before the first frame update
void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);
        transform.position += new Vector3(movement.x, movement.y, 0) * Time.deltaTime * moveSpeed;

        FireAutoCannon();
        FireHomingProjectile();
        FireHealingDrone();
    }
}
