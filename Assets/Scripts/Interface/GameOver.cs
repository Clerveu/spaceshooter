using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.lives = 3;
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
        GameManager.Instance.isGameOver = false;
        yield return new WaitForSecondsRealtime(7f);
        AudioManager.instance.StopMusic("gameover", 0f);
        SceneManager.LoadScene(1);
    }

}
