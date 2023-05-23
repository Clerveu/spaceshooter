using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Button initialButton;

    private void OnEnable()
    {
        // Set the initial button as the selected game object
        EventSystem.current.SetSelectedGameObject(initialButton.gameObject);
    }
   
}
