using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuJukebox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.StopAll();
        StartCoroutine(MainMenuMusic());
    }

    IEnumerator MainMenuMusic()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayMusic("mainmusic", 0f);
        Debug.Log("MainMenuJukebox Requested mainmusic");
    }
}
