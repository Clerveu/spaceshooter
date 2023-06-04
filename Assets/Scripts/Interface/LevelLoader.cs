using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public float delay = 3f;
    public string scene;


    void Start()
    {
        StartCoroutine(DelayAndLoad());
    }

    IEnumerator DelayAndLoad()
    {
        yield return new WaitForSeconds(delay);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
    }
}
