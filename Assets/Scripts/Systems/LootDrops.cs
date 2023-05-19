using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DropItem
{
    public GameObject dropPrefab;
    public int baseDropThreshold;
    public int dropThreshold;
    public int dropRange; // Add this new variable here
    public bool dropEveryTime;
}

public class LootDrops : MonoBehaviour
{
    [SerializeField]
    private DropItem[] dropItems;

    private static Dictionary<int, int> dropCounts = new Dictionary<int, int>();
    private static Dictionary<int, bool> hasDroppedOnce = new Dictionary<int, bool>();

    private void Start()
    {
        for (int i = 0; i < dropItems.Length; i++)
        {
            dropItems[i].dropThreshold = Random.Range(dropItems[i].baseDropThreshold - dropItems[i].dropRange, dropItems[i].baseDropThreshold + dropItems[i].dropRange + 1);
        }
    }

    private void OnDestroy()
    {
        // Get the flag indicating whether the object was destroyed by the player or through self-destruction
        Health health = GetComponent<Health>();
        bool destroyedByPlayer = health != null && health.destroyedByPlayer;
        bool selfDestructed = GetComponent<SelfDestruct>() != null && GetComponent<SelfDestruct>().selfDestructed;

        // Only perform the loot drop if the object was destroyed by the player
        if (destroyedByPlayer && !selfDestructed)
        {
            for (int i = 0; i < dropItems.Length; i++)
            {
                if (!dropCounts.ContainsKey(i))
                {
                    dropCounts[i] = 0;
                    Debug.Log($"Initializing drop count for item {i}");
                }

                if (!hasDroppedOnce.ContainsKey(i))
                {
                    hasDroppedOnce[i] = false;
                    Debug.Log($"Initializing hasDroppedOnce for item {i}");
                }

                dropCounts[i]++;

                if (dropCounts[i] >= dropItems[i].dropThreshold)
                {
                    if (!dropItems[i].dropEveryTime && hasDroppedOnce[i])
                    {
                        Debug.Log($"Skipping drop for item {i}: should not drop every time and has already dropped once");
                        continue; // Skip the item if it shouldn't drop every time and has already dropped once
                    }

                    if (dropItems[i].dropPrefab == null)
                    {
                        Debug.LogWarning($"Drop item {i} has null dropPrefab");
                        continue; // Skip the item if it has a null dropPrefab
                    }

                    if (transform.position == null)
                    {
                        Debug.LogError($"Transform position is null");
                        continue; // Skip the item if transform position is null
                    }

                    Instantiate(dropItems[i].dropPrefab, transform.position, Quaternion.identity);

                    if (dropItems[i].dropEveryTime)
                    {
                        dropCounts[i] -= dropItems[i].dropThreshold;
                    }
                    else
                    {
                        dropCounts[i] = 0; // Reset the count for future drops
                        hasDroppedOnce[i] = true;
                    }

                    // Randomize the drop threshold for the next drop
                    dropItems[i].dropThreshold = Random.Range(dropItems[i].baseDropThreshold - dropItems[i].dropRange, dropItems[i].baseDropThreshold + dropItems[i].dropRange + 1);
                    Debug.Log("New drop threshold for item " + i + ": " + dropItems[i].dropThreshold);
                }
            }
        }
    }
}
