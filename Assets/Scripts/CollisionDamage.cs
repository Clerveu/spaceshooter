using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour

{
    public float damageAmount = 10f;
    private Rigidbody2D rb;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Health playerShipHealth = GetComponent<Health>();
            if (playerShipHealth != null)
            {
                playerShipHealth.TakeDamage(damageAmount);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
