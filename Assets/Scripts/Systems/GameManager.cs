using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int lives = 3;
    public bool enableScreenShake = true;
    public List<Camera> cameras;
    public float currentFOV = 60f;
    public bool isExitingToMenu = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnDeath()
    {
        if (isExitingToMenu) return;

        lives--;

        // Stop the music when the player dies
        AudioManager.instance.StopMusic("levelmusic", 0f);
        Time.timeScale = 0f;

        if (lives > 0)
        {
            StartCoroutine(LevelReset());
        }
        else
        {
            StartCoroutine(GameOver());
        }
    }

    public void UpdateFOV(float fov)
    {
        currentFOV = fov;

        // Update the FOV of each camera
        foreach (Camera camera in cameras)
        {
            camera.orthographicSize = fov;
        }
    }

    public float GetFOV()
    {
        return currentFOV;
    }

    // Call this when you enable a new camera
    public void RegisterCamera(Camera newCamera)
    {
        if (!cameras.Contains(newCamera))
        {
            cameras.Add(newCamera);
            newCamera.orthographicSize = currentFOV; // Update the new camera's FOV
        }
    }

    // Call this when you disable a camera
    public void UnregisterCamera(Camera oldCamera)
    {
        if (cameras.Contains(oldCamera))
        {
            cameras.Remove(oldCamera);
        }
    }


    public void YouWin()
    {
        AudioManager.instance.StopMusic("bossmusic", 3f);
        StartCoroutine(GoToCredits());
    }

    IEnumerator GoToCredits()
    {
        yield return new WaitForSecondsRealtime(3);
        lives = 3;
        SceneManager.LoadScene(5);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(3f);
        lives = 3;
        Time.timeScale = 1f;
        SceneManager.LoadScene(4);
    }

    IEnumerator LevelReset()
    {
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(2); // or any other scene you want to load
    }
}
