using UnityEngine;
using UnityEngine.UI;

public class ScreenShakeToggleHandler : MonoBehaviour
{
    public Toggle ScreenShakeToggle;

    private void Start()
    {
        // Initialize the toggle's value
        ScreenShakeToggle.isOn = GameManager.Instance.enableScreenShake;

        // Add a listener to the toggle's value changed event
        ScreenShakeToggle.onValueChanged.AddListener(HandleScreenShakeToggleChanged);
    }

    private void HandleScreenShakeToggleChanged(bool newValue)
    {
        GameManager.Instance.enableScreenShake = newValue;
    }
}
