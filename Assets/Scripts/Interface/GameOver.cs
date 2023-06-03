using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GameOverMusic());
    }

    IEnumerator GameOverMusic()
    {
        yield return new WaitForSecondsRealtime(1f);
        AudioManager.instance.PlayMusic("gameover", 0f);
        StartCoroutine(GoToStart());
    }

    IEnumerator GoToStart()
    {
        yield return new WaitForSecondsRealtime(7f);
        SceneManager.LoadScene(1);
        AudioManager.instance.StopAll();
    }

}
