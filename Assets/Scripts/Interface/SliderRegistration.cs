using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderRegistration : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider fovSlider;

    private void Start()
    {
        SliderManager.instance.RegisterSlider(masterVolumeSlider, SliderHandler.SliderType.Master);
        SliderManager.instance.RegisterSlider(sfxVolumeSlider, SliderHandler.SliderType.SFX);
        SliderManager.instance.RegisterSlider(musicVolumeSlider, SliderHandler.SliderType.Music);
        SliderManager.instance.RegisterSlider(fovSlider, SliderHandler.SliderType.FOV);
    }
}
