using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class VolumeControls : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset volumeControlsAsset;
    [SerializeField] private AudioMixer mixer;

    private VisualElement volumeControls;
    private Slider masterVolumeSlider, bgVolumeSlider, sfxVolumeSlider, voiceVolumeSlider;
    private static int minVolume = -80;
    private static int maxVolume = 0;
    private static int defaultVolume = 0;

    private void Awake()
    {
        // volume controls
        volumeControls = volumeControlsAsset.Instantiate();
        masterVolumeSlider = volumeControls.Q<Slider>("MasterVolumeSlider");
        bgVolumeSlider = volumeControls.Q<Slider>("BgVolumeSlider");
        sfxVolumeSlider = volumeControls.Q<Slider>("SFXVolumeSlider");
        voiceVolumeSlider = volumeControls.Q<Slider>("VoiceVolumeSlider");
        List<Slider> sliders = new List<Slider>()
        {
            masterVolumeSlider, bgVolumeSlider, sfxVolumeSlider, voiceVolumeSlider
        };

        foreach (Slider slider in sliders)
        {
            slider.lowValue = minVolume;
            slider.highValue = maxVolume;
            slider.value = defaultVolume;
        }

        masterVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnMasterVolumeChanged);
        bgVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnBgVolumeChanged);
        sfxVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnSfxVolumeChanged);
        voiceVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnVoiceVolumeChanged);
    }

    public VisualElement GetVolumeControlElement()
    {
        return volumeControls;
    }
    private void OnMasterVolumeChanged(ChangeEvent<float> e)
    {
        mixer.SetFloat("MasterVolume", e.newValue);
        if (e.newValue == minVolume)
        {
            EnableChildSliders(false);
        }
        else if (e.previousValue == minVolume)
        {
            EnableChildSliders(true);
        }
    }

    private void OnBgVolumeChanged(ChangeEvent<float> e)
    {
        mixer.SetFloat("BGVolume", e.newValue);
    }

    private void OnSfxVolumeChanged(ChangeEvent<float> e)
    {
        mixer.SetFloat("SFXVolume", e.newValue);
    }

    private void OnVoiceVolumeChanged(ChangeEvent<float> e)
    {
        mixer.SetFloat("VoiceVolume", e.newValue);
    }

    private void EnableChildSliders(bool enabled)
    {
        bgVolumeSlider.SetEnabled(enabled);
        sfxVolumeSlider.SetEnabled(enabled);
        voiceVolumeSlider.SetEnabled(enabled);
    }
}
