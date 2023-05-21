using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { HomingProjectile, HealingDrone }
    public PowerUpType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        Debug.Log("player matched");
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.PickUpPowerUp(type);
                AudioManager.instance.Play("powerup");
                Destroy(gameObject);
            }
        }
    }
}
