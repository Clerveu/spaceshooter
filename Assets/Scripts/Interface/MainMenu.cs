using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;

    private void OnEnable()
    {
        // Set the initial button as the selected game object
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

}
