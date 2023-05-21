using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 0.7f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;
        public float initialVolume;
        [HideInInspector]
        public AudioSource source;
    }

    [System.Serializable]
    public class Music
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 0.7f;
        public float initialVolume;
        [HideInInspector]
        public AudioSource source;
    }


    public Sound[] sounds;
    public Music[] musics;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.initialVolume = s.volume;
        }

        foreach (Music m in musics)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.loop = true;
            m.initialVolume = m.volume;
        }

    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();


    }

    public void PlayMusic(string name, float duration)
    {
        Music m = System.Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }

        StartCoroutine(FadeInRoutine(m.source, duration));
    }

    public void StopMusic(string name, float duration)
    {
        Music m = System.Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }

        StartCoroutine(FadeOutRoutine(m.source, duration));
    }

    IEnumerator FadeInRoutine(AudioSource audioSource, float duration)
    {
        if (duration <= 0)
        {
            audioSource.Play();
        }
        else
        {
            float currentTime = 0;
            float initialVolume = audioSource.volume;

            audioSource.volume = 0;
            audioSource.Play();

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0, initialVolume, currentTime / duration);
                yield return null;
            }

            audioSource.volume = initialVolume;
        }
    }

    IEnumerator FadeOutRoutine(AudioSource audioSource, float duration)
    {
        if (duration <= 0)
        {
            audioSource.Stop();
        }
        else
        {
            float currentTime = 0;
            float initialVolume = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(initialVolume, 0, currentTime / duration);
                yield return null;
            }

            audioSource.volume = 0;
            audioSource.Stop();
        }
    }




}