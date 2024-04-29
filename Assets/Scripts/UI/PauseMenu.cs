using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private UIDocument document;
    private VisualElement wrapper;
    private Slider masterVolumeSlider, bgVolumeSlider, sfxVolumeSlider, voiceVolumeSlider;
    private Button restartBtn;
    private Button backBtn;

    [SerializeField]
    private AudioMixer mixer;
    private static int minVolume = -80;
    private static int maxVolume = 0;
    private static int defaultVolume = 0;

    public EventBool PauseMenuToggleEvent;
    public EventBool EnableCamControlEvent;
    public EventBool CursorVisibleEvent;

    public PlayerData PlayerData;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        wrapper = document.rootVisualElement.Q("Wrapper");

        restartBtn = wrapper.Q<Button>("RestartBtn");
        backBtn = wrapper.Q<Button>("BackBtn");

        SetVisible(false);
        backBtn.RegisterCallback<ClickEvent>(OnBackBtnClicked);
        restartBtn.RegisterCallback<ClickEvent>(OnRestart);

        // volume controls
        masterVolumeSlider = wrapper.Q<Slider>("MasterVolumeSlider");
        bgVolumeSlider = wrapper.Q<Slider>("BgVolumeSlider");
        sfxVolumeSlider = wrapper.Q<Slider>("SFXVolumeSlider");
        voiceVolumeSlider = wrapper.Q<Slider>("VoiceVolumeSlider");
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

    private void OnEnable()
    {
        PauseMenuToggleEvent.OnEventRaised += SetVisible;
    }

    private void OnDisable()
    {
        PauseMenuToggleEvent.OnEventRaised -= SetVisible;
    }

    private void SetVisible(bool visible)
    {
        if (visible)
        {
            // pause
            CursorVisibleEvent?.RaiseEvent(true);
            PlayerData.MovementEnabled = false;
            EnableCamControlEvent.RaiseEvent(false);
            Time.timeScale = 0;
        }
        else
        {
            // unpause
            CursorVisibleEvent?.RaiseEvent(false);
            PlayerData.MovementEnabled = true;
            EnableCamControlEvent.RaiseEvent(true);
            Time.timeScale = 1;
        }

        wrapper.visible = visible;
    }

    private void OnBackBtnClicked(ClickEvent e)
    {
        PauseMenuToggleEvent.RaiseEvent(false);
    }

    private void OnRestart(ClickEvent e)
    {
        // todo 
        SceneManager.LoadScene("MenuScreen");
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
