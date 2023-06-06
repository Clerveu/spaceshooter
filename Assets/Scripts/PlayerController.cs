using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public GameObject autoCannonPrefab;
    public GameObject homingProjectilePrefab;
    public GameObject healingDronePrefab;
    public Canvas canvas;
    public Sprite homingProjectileIcon;
    public Sprite healingDroneIcon;
    public float autoCannonFireRate = 10f;
    public float homingProjectileFireRate = 100f;
    public float healingDroneFireRate = 1f;
    public float moveSpeed = 5f;
    public float iconStartX = 10f;
    public float iconStartY = 10f;
    public float iconSpacingX = 5f;
    public float iconSpacingY = 5f;
    private InputMaster inputMaster;
    private List<Image> specialWeaponIcons = new List<Image>();
    private GamePause gamePause;

    private float nextAutoCannonFireTime;
    private float nextHomingProjectileFireTime;
    private float nextHealingDroneFireTime;

    public enum SpecialWeapon { None, HomingProjectile, HealingDrone }

    private int currentSpecialWeaponIndex = -1;
    private SpecialWeapon currentSpecialWeapon = SpecialWeapon.None;

    private bool hasHomingProjectile;
    private bool hasHealingDrone;
    public List<SpecialWeapon> collectedSpecialWeapons = new List<SpecialWeapon>();

    void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.X.performed += ctx => FireCurrentSpecialWeapon();
        inputMaster.Player.A.performed += ctx => FireAutoCannon();
        inputMaster.Player.RBumper.performed += ctx => CycleSpecialWeaponForward();
        inputMaster.Player.LBumper.performed += ctx => CycleSpecialWeaponBackward();
        inputMaster.Player.Menu.performed += ctx => TogglePause();
    }

    public void PickUpPowerUp(PowerUp.PowerUpType type)
    {
        SpecialWeapon specialWeapon = SpecialWeapon.None;
        switch (type)
        {
            case PowerUp.PowerUpType.HomingProjectile:
                hasHomingProjectile = true;
                specialWeapon = SpecialWeapon.HomingProjectile;
                break;
            case PowerUp.PowerUpType.HealingDrone:
                hasHealingDrone = true;
                specialWeapon = SpecialWeapon.HealingDrone;
                break;
        }

        if (specialWeapon != SpecialWeapon.None && !collectedSpecialWeapons.Contains(specialWeapon))
        {
            collectedSpecialWeapons.Add(specialWeapon);
            if (currentSpecialWeapon == SpecialWeapon.None)
            {
                currentSpecialWeaponIndex = 0;
                currentSpecialWeapon = collectedSpecialWeapons[currentSpecialWeaponIndex];
            }

            // Add the corresponding icon for the powerup
            switch (specialWeapon)
            {
                case SpecialWeapon.HomingProjectile:
                    AddPowerUpIcon(homingProjectileIcon);
                    break;
                case SpecialWeapon.HealingDrone:
                    AddPowerUpIcon(healingDroneIcon);
                    break;
            }
        }
    }

    public List<SpecialWeapon> GetCollectedWeapons()
    {
        // This method simply returns the list of collected special weapons
        return collectedSpecialWeapons;
    }

    private void FireCurrentSpecialWeapon()
    {
        FireSpecialWeapon(currentSpecialWeapon);
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    private void CycleSpecialWeapon(int direction)
    {
        if (collectedSpecialWeapons.Count == 0) return;

        int oldIndex = currentSpecialWeaponIndex;
        currentSpecialWeaponIndex = (currentSpecialWeaponIndex + direction + collectedSpecialWeapons.Count) % collectedSpecialWeapons.Count;

        UpdateIconVisibility(oldIndex, currentSpecialWeaponIndex);

        currentSpecialWeapon = collectedSpecialWeapons[currentSpecialWeaponIndex];
    }

    private void UpdateIconVisibility(int oldIndex, int newIndex)
    {
        for (int i = 0; i < specialWeaponIcons.Count; i++)
        {
            if (i == newIndex)
            
            {
                specialWeaponIcons[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                specialWeaponIcons[i].color = new Color(1, 1, 1, 0);
            }
        }
    }


    private void CycleSpecialWeaponForward()
    {
        if (Time.timeScale == 0) return;
        CycleSpecialWeapon(1);
    }

    private void CycleSpecialWeaponBackward()
    {
        if (Time.timeScale == 0) return;
        CycleSpecialWeapon(-1);
    }

    private void FireSpecialWeapon(SpecialWeapon weapon)
    {
        switch (weapon)
        {
            case SpecialWeapon.HomingProjectile:
                FireHomingProjectile();
                break;
            case SpecialWeapon.HealingDrone:
                FireHealingDrone();
                break;
            default:
                break;
        }
    }

    private void FireAutoCannon()
    {
        if (Time.timeScale == 0) return;
        if (Time.time > nextAutoCannonFireTime)
        {
            nextAutoCannonFireTime = Time.time + 1f / autoCannonFireRate;
            GameObject bullet = Instantiate(autoCannonPrefab, transform.position + transform.right * 0.5f, Quaternion.identity);
            AudioManager.instance.Play("pew");
            if (bullet != null)
            {
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = transform.right * 5;
            }
        }
    }

    private void FireHomingProjectile()
    {
        if (Time.timeScale == 0) return;
        if (Time.time > nextHomingProjectileFireTime)
        {
            nextHomingProjectileFireTime = Time.time + 1f / homingProjectileFireRate;
            GameObject homingProjectile = Instantiate(homingProjectilePrefab, transform.position + transform.right * 0.5f, Quaternion.identity);
            AudioManager.instance.Play("rocketshoot");
            if (homingProjectile != null)
            {
                Rigidbody2D homingProjectileRigidbody = homingProjectile.GetComponent<Rigidbody2D>();
                homingProjectileRigidbody.velocity = transform.right * 1;
            }
        }
    }

    private void FireHealingDrone()
    {
        if (Time.timeScale == 0) return;
        if (Time.time > nextHealingDroneFireTime)
        {
            nextHealingDroneFireTime = Time.time + 1f / healingDroneFireRate;
            GameObject healingDrone = Instantiate(healingDronePrefab, transform.position + transform.right * 0.5f, Quaternion.identity);
            HealingDrone healingDroneComponent = healingDrone.GetComponent<HealingDrone>();
            AudioManager.instance.Play("healingdrone");
            if (healingDroneComponent != null)
            {
                healingDroneComponent.InitializeDrone(gameObject);

            }
        }
    }

    private void AddPowerUpIcon(Sprite iconSprite)
    {
        GameObject powerUpIcon = new GameObject("PowerUpIcon");
        powerUpIcon.transform.SetParent(UIManager.instance.canvas.transform, false);

        Image powerUpImage = powerUpIcon.AddComponent<Image>();
        powerUpImage.sprite = iconSprite;
        // You can change the Vector2 values (50, 50) to resize the icon
        powerUpImage.rectTransform.sizeDelta = new Vector2(50, 50);

        // Position the icon at the same location
        powerUpImage.rectTransform.anchoredPosition = new Vector2(iconStartX, iconStartY);
        // Set the icon's transparency to 0 if it's not the current special weapon
        if (collectedSpecialWeapons.Count != currentSpecialWeaponIndex + 1)
        {
            powerUpImage.color = new Color(1, 1, 1, 0);
        }
        specialWeaponIcons.Add(powerUpImage);
    }

    void Start()
    {
        gamePause = GamePause.instance;
        Health health = GetComponent<Health>();
        health.OnDeath += OnDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        var gamepad = Gamepad.current;
        if (gamepad == null) return;

        Vector2 movement = gamepad.leftStick.ReadValue();
        transform.position += new Vector3(movement.x, movement.y, 0) * Time.deltaTime * moveSpeed;

        if (gamepad.buttonSouth.IsPressed())
        {
            FireAutoCannon();
        }        
    }

    void TogglePause()
    {
        if (GamePause.instance == null)
        {
            Debug.LogError("GamePause instance is null!");
            return;
        }

        if (Time.timeScale == 1)
        {
            GamePause.instance.PauseGame();
        }
        else
        {
            GamePause.instance.UnpauseGame();
        }
    }

    private void OnDeath()
    {
 //       GameManager.Instance.DecreaseLife();
    }

    private void OnDestroy()
    {
        Health health = GetComponent<Health>();
        health.OnDeath -= OnDeath;
        GameManager.Instance.OnDeath();
    }

    public void AddSpecialWeapon(SpecialWeapon weapon)
    {
        // If the weapon is not already in the collectedSpecialWeapons list, add it
        if (!collectedSpecialWeapons.Contains(weapon))
        {
            collectedSpecialWeapons.Add(weapon);

            // If this is the first weapon collected, make it the current weapon
            if (currentSpecialWeapon == SpecialWeapon.None)
            {
                currentSpecialWeaponIndex = 0;
                currentSpecialWeapon = collectedSpecialWeapons[currentSpecialWeaponIndex];
            }

            // Add the corresponding icon for the powerup
            switch (weapon)
            {
                case SpecialWeapon.HomingProjectile:
                    AddPowerUpIcon(homingProjectileIcon);
                    break;
                case SpecialWeapon.HealingDrone:
                    AddPowerUpIcon(healingDroneIcon);
                    break;
            }
        }
    }

}
