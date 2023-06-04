using UnityEngine;

public class LivesDisplay : MonoBehaviour
{
    public Sprite[] lifeSprites; // assign in the Inspector
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void Update()
    {
        // Update the sprite if the number of lives has changed
        if (GameManager.Instance.lives < lifeSprites.Length)
        {
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        if (GameManager.Instance.lives <= 0)
        {
            spriteRenderer.sprite = null; // hide the sprite
        }
        else
        {
            spriteRenderer.sprite = lifeSprites[GameManager.Instance.lives - 1];
        }
    }
}
