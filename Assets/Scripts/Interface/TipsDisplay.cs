using TMPro;
using UnityEngine;

public class TipsDisplay : MonoBehaviour
{
    public string[] tips; // assign your tips in the Inspector
    private TextMeshPro textMeshPro;
    private bool hasDisplayedTip = false;

    private void Start()
    {
        LootDrops.ResetDropCounts();
        textMeshPro = GetComponent<TextMeshPro>();
        if (!hasDisplayedTip) // Check if a tip has been displayed before
        {
            DisplayRandomTip();
            hasDisplayedTip = true; // Set this to true after displaying a tip
        }
    }

    private void DisplayRandomTip()
    {
        int randomIndex = Random.Range(0, tips.Length);
        textMeshPro.text = tips[randomIndex];
    }
}
