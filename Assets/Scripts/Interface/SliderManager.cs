using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public static SliderManager instance;

    Slider masterVolumeSlider;
    Slider sfxVolumeSlider;
    Slider musicVolumeSlider;
    Slider fovSlider;


    private void Awake()
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
    }

    public void RegisterSlider(Slider slider, SliderHandler.SliderType type)
    {
        var handler = slider.gameObject.AddComponent<SliderHandler>();
        handler.sliderType = type;

        switch (type)
        {
            case SliderHandler.SliderType.Master:
                masterVolumeSlider = slider;
                masterVolumeSlider.onValueChanged.AddListener(AudioManager.instance.UpdateMasterVolume);
                break;
            case SliderHandler.SliderType.SFX:
                sfxVolumeSlider = slider;
                sfxVolumeSlider.onValueChanged.AddListener(AudioManager.instance.UpdateSFXVolume);
                break;
            case SliderHandler.SliderType.Music:
                musicVolumeSlider = slider;
                musicVolumeSlider.onValueChanged.AddListener(AudioManager.instance.UpdateMusicVolume);
                break;
            case SliderHandler.SliderType.FOV:
                fovSlider = slider;
                fovSlider.onValueChanged.AddListener(GameManager.Instance.UpdateFOV);
                break;
        }
    }


    public void SetMasterVolumeSlider(Slider newSlider)
    {
        masterVolumeSlider = newSlider;
        masterVolumeSlider.onValueChanged.AddListener(AudioManager.instance.UpdateMasterVolume);
    }

    public void SetSFXVolumeSlider(Slider newSlider)
    {
        sfxVolumeSlider = newSlider;
        sfxVolumeSlider.onValueChanged.AddListener(AudioManager.instance.UpdateSFXVolume);
    }

    public void SetMusicVolumeSlider(Slider newSlider)
    {
        musicVolumeSlider = newSlider;
        musicVolumeSlider.onValueChanged.AddListener(AudioManager.instance.UpdateMusicVolume);
    }
}
