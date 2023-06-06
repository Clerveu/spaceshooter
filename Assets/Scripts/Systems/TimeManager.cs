using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    private float gameTime;

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

    private void Update()
    {
        // Increment the gameTime by the time elapsed since last frame
        gameTime += Time.deltaTime;
    }

    public float GetTime()
    {
        return gameTime;
    }

    public void SetTime(float time)
    {
        gameTime = time;
    }
}
