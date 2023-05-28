using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    public enum SliderType { Master, SFX, Music, FOV }
    public SliderType sliderType;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        // Initialize the slider value based on the current volumes in AudioManager
        switch (sliderType)
        {
            case SliderType.Master:
                slider.value = AudioManager.instance.GetMasterVolume();
                break;
            case SliderType.SFX:
                slider.value = AudioManager.instance.GetSFXVolume();
                break;
            case SliderType.Music:
                slider.value = AudioManager.instance.GetMusicVolume();
                break;
            case SliderType.FOV:
                slider.value = GameManager.Instance.GetFOV();
                break;
        }
    }
}
