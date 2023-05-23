using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Static instance
    public static SceneLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneIndexToLoad)
    {
        StartCoroutine(LoadSceneAsync(sceneIndexToLoad));
    }

    private IEnumerator LoadSceneAsync(int sceneIndexToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndexToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
