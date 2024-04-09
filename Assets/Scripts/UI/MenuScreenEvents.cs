using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuScreenEvents : MonoBehaviour
{
    private UIDocument document;
    private Button startBtn;
    private Button settingsBtn;

    void Awake()
    {
        document = GetComponent<UIDocument>();
        startBtn = document.rootVisualElement.Q("StartBtn") as Button;
        settingsBtn = document.rootVisualElement.Q("SettingsBtn") as Button;

        startBtn.RegisterCallback<PointerUpEvent>(OnClickStartBtn);
        settingsBtn.RegisterCallback<PointerUpEvent>(OnClickSettingsBtn);
    }

    private void OnClickStartBtn(PointerUpEvent e)
    {
        Debug.Log("welcome to the zone");
    }

    private void OnClickSettingsBtn(PointerUpEvent e)
    {
        Debug.Log("zone settings");
    }

    private void OnDisable()
    {
        startBtn.UnregisterCallback<PointerUpEvent>(OnClickStartBtn);
        settingsBtn.UnregisterCallback<PointerUpEvent>(OnClickSettingsBtn);
    }
}
