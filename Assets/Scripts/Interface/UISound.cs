using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    public void ClickSound()
    {
        AudioManager.instance.Play("click");
    }

    public void SelectSound()
    {
        AudioManager.instance.Play("select");
    }

    public void StartSound()
    {
        AudioManager.instance.Play("startgame");
    }

    public void FadeMusic()
    {
        AudioManager.instance.StopMusic("mainmusic", 1.5f);
    }
}
