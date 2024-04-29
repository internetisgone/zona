using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private UIDocument document;
    private VisualElement wrapper;
    private Slider masterVolumeSlider;
    private Button restartBtn;
    private Button backBtn;

    public EventBool PauseMenuToggleEvent;
    public EventBool EnableCamControlEvent;
    public EventBool CursorVisibleEvent;

    public PlayerData PlayerData;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        wrapper = document.rootVisualElement.Q("Wrapper");
        masterVolumeSlider = wrapper.Q<Slider>("MasterVolumeSlider");
        restartBtn = wrapper.Q<Button>("RestartBtn");
        backBtn = wrapper.Q<Button>("BackBtn");

        SetVisible(false);
        backBtn.RegisterCallback<ClickEvent>(OnBackBtnClicked);
        restartBtn.RegisterCallback<ClickEvent>(OnRestart);
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
}
