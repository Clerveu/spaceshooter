using UnityEngine;


public class GamePause : MonoBehaviour
{
    public static GamePause instance;
    public GameObject MenuScreen;
    public GameObject MainCamera;
    public GameObject MenuCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Call this function to toggle pause state
    public void PauseGame()
    {
        Debug.Log("PauseGame called in GamePause");
        Time.timeScale = 0;
        MainCamera.SetActive(false);
        MenuCamera.SetActive(true);
        AudioManager.instance.PauseAll();
        MenuScreen.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        MainCamera.SetActive(true);
        MenuCamera.SetActive(false);
        AudioManager.instance.UnpauseAll();
        MenuScreen.SetActive(false);
    }
}
