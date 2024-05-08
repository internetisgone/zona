using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public PlayerData playerData;
    public NPCData npcData;

    private UIDocument document;

    // menu view
    private VisualElement menuView;
    private Button startBtn;
    private Button settingsBtn;

    // settings view
    private VisualElement settingsView;
    private VisualElement volumeControls;
    private Slider mouseSensitivitySlider;
    private Slider npcSpeedSlider;
    private Button confirmBtn;
    private Button resetBtn;

    void Awake()
    {
        document = GetComponent<UIDocument>();

        menuView = document.rootVisualElement.Q("MenuWrapper");
        startBtn = document.rootVisualElement.Q<Button>("StartBtn");
        settingsBtn = document.rootVisualElement.Q<Button>("SettingsBtn");

        settingsView = document.rootVisualElement.Q("SettingsWrapper");
        mouseSensitivitySlider = document.rootVisualElement.Q<Slider>("MouseSensitivitySlider");
        npcSpeedSlider = document.rootVisualElement.Q<Slider>("NPCSpeedSlider");
        confirmBtn = document.rootVisualElement.Q<Button>("ConfirmBtn");
        resetBtn = document.rootVisualElement.Q<Button>("ResetBtn");

        // show menu view by default
        settingsView.visible = false;
        menuView.visible = true;

        // init slider constraints and default values
        mouseSensitivitySlider.lowValue = 0.1f;
        mouseSensitivitySlider.highValue = 2f;
        npcSpeedSlider.lowValue = 1f;
        npcSpeedSlider.highValue = 10f;
        SetToDefault();

        // register callback
        startBtn.RegisterCallback<PointerUpEvent>(OnStartClicked);
        settingsBtn.RegisterCallback<PointerUpEvent>(OnSettingsClicked);
        confirmBtn.RegisterCallback<PointerUpEvent>(OnConfirmSettings);
        resetBtn.RegisterCallback<PointerUpEvent>(OnResetSettings);

        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        volumeControls = gameObject.GetComponent<VolumeControls>()?.GetVolumeControlElement();
        settingsView.Insert(1, volumeControls);
    }
    private void OnStartClicked(PointerUpEvent e)
    {
        SceneManager.LoadScene("Zona");
    }

    private void OnSettingsClicked(PointerUpEvent e)
    {
        menuView.visible = false;
        settingsView.visible = true;
    }

    private void OnConfirmSettings(PointerUpEvent e)
    {
        playerData.MouseSensitivity = mouseSensitivitySlider.value;
        npcData.Speed = npcSpeedSlider.value;
        settingsView.visible = false;
        menuView.visible = true;
    }

    private void OnResetSettings(PointerUpEvent e)
    {
        SetToDefault();
    }

    private void SetToDefault()
    {
        playerData.Reset();
        npcData.Reset();
        mouseSensitivitySlider.value = playerData.MouseSensitivity;
        npcSpeedSlider.value = npcData.Speed;
    }

    private void OnDisable()
    {
        startBtn.UnregisterCallback<PointerUpEvent>(OnStartClicked);
        settingsBtn.UnregisterCallback<PointerUpEvent>(OnSettingsClicked);
        confirmBtn.UnregisterCallback<PointerUpEvent>(OnConfirmSettings);
        resetBtn.UnregisterCallback<PointerUpEvent>(OnResetSettings); ;
    }
}