using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FPSCounter : MonoBehaviour
{
    private UIDocument document;
    private Label fpsText;
    private Label frameDutaionText;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        fpsText = document.rootVisualElement.Q<Label>("FPSValue");
        frameDutaionText = document.rootVisualElement.Q<Label>("FrameDuration");
    }

    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        fpsText.text = (1f / frameDuration).ToString("0");
        frameDutaionText.text = (frameDuration * 1000).ToString("0.00");
    }
}
