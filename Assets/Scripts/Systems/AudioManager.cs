using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public float masterVolume = 1f;
    public float sfxVolume = 1f;
    public float musicVolume = 1f;

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

    public AudioSource Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        s.source.Play();
        return s.source;
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
        float initialVolume = audioSource.volume;
        if (duration <= 0)
        {
            audioSource.Stop();
        }
        else
        {
            float currentTime = 0;
           

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(initialVolume, 0, currentTime / duration);
                yield return null;
            }

            audioSource.Stop();
        }

        audioSource.volume = initialVolume;
    }

    public void PauseAll()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
                s.source.Pause();
        }

        foreach (Music m in musics)
        {
            if (m.source.isPlaying)
                m.source.Pause();
        }
    }

    public void UnpauseAll()
    {
        foreach (Sound s in sounds)
        {
            if (!s.source.isPlaying)
                s.source.UnPause();
        }

        foreach (Music m in musics)
        {
            if (!m.source.isPlaying)
                m.source.UnPause();
        }
    }

    public void StopAllMusic()
    {
        foreach (Music m in musics)
        {
            m.source.Stop();
        }
    }

    public void StopAllSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void StopAll()
    {
        StopAllMusic();
        StopAllSounds();
    }

    public void SetSoundEffectsVolume(float volume)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.initialVolume * volume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        foreach (Music m in musics)
        {
            m.source.volume = m.initialVolume * volume;
        }
    }

    public void UpdateMasterVolume(float volume)
    {
        masterVolume = volume;

        // update the volume of all sounds and music
        foreach (Sound s in sounds)
        {
            s.source.volume = s.initialVolume * sfxVolume * masterVolume;
        }
        foreach (Music m in musics)
        {
            m.source.volume = m.initialVolume * musicVolume * masterVolume;
        }
    }

    public void UpdateSFXVolume(float volume)
    {
        sfxVolume = volume;

        // update the volume of all sounds
        foreach (Sound s in sounds)
        {
            s.source.volume = s.initialVolume * sfxVolume * masterVolume;
        }
    }

    public void UpdateMusicVolume(float volume)
    {
        musicVolume = volume;

        // update the volume of all music
        foreach (Music m in musics)
        {
            m.source.volume = m.initialVolume * musicVolume * masterVolume;
        }
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void MusicBandaid()
    {
        foreach (Music m in musics)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.loop = true;
        }
    }
}