using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Jukebox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Level1Music());
        StartCoroutine(Level1BossMusic());
    }

    IEnumerator Level1Music()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayMusic("levelmusic", 0f);
        StartCoroutine(Level1MusicStop());
    }

    IEnumerator Level1MusicStop()
    {
        yield return new WaitForSeconds(125f);
        AudioManager.instance.StopMusic("levelmusic", 3f);
    }

    IEnumerator Level1BossMusic()
    {
        yield return new WaitForSeconds(130f);
        //AudioManager.instance.PlayMusic("bossmusic", 0f);
    }
}
