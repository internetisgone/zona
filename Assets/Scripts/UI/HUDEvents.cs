using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDEvents : MonoBehaviour
{
    private UIDocument document;
    private Label collectTextTip;
    private Label counter;

    public EventVoid ArtifactIsCollectible;
    public EventVoid ArtifactNoLongerCollectible;

    public EventInt SetArtifactCount;

    void Awake()
    {
        document = GetComponent<UIDocument>();
        collectTextTip = document.rootVisualElement.Q("CollectText") as Label;
        counter = document.rootVisualElement.Q("Quantity") as Label;
    }

    private void OnEnable()
    {
        ArtifactIsCollectible.OnEventRaised += ShowCollectText;
        ArtifactNoLongerCollectible.OnEventRaised += HideCollectText;

        SetArtifactCount.OnEventRaised += UpdateArtifactCounter;
    }

    private void OnDisable()
    {
        ArtifactIsCollectible.OnEventRaised -= ShowCollectText;
        ArtifactNoLongerCollectible.OnEventRaised -= HideCollectText;

        SetArtifactCount.OnEventRaised -= UpdateArtifactCounter;
    }

    public void ShowCollectText()
    {
        collectTextTip.visible = true;
    }

    public void HideCollectText()
    {
        collectTextTip.visible = false;
    }

    public void UpdateArtifactCounter(int count) 
    {
        counter.text = count.ToString();
    }
}
