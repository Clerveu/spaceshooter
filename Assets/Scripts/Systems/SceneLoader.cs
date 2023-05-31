using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

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

    public static void LoadScene(int sceneIndexToLoad)
    {
        StackTrace stackTrace = new StackTrace();
        UnityEngine.Debug.Log("LoadScene was called by: " + stackTrace.GetFrame(1).GetMethod().Name);
        instance.StartCoroutine(instance.LoadSceneAsync(sceneIndexToLoad));
    }


    private IEnumerator LoadSceneAsync(int sceneIndexToLoad)
    {
        StackTrace stackTrace = new StackTrace();
        UnityEngine.Debug.Log("LoadSceneAsync was called by: " + stackTrace.GetFrame(1).GetMethod().Name);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndexToLoad);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
