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
    private VisualElement proximityWrapper;
    private Label proximityIndicator;

    public EventVoid ArtifactIsCollectible;
    public EventVoid ArtifactNoLongerCollectible;

    public EventInt ArtifactCountUpdated;
    public EventFloat ArtifactProximityUpdated;

    public EventBool DetectorEquipped;

    void Awake()
    {
        document = GetComponent<UIDocument>();
        collectTextTip = document.rootVisualElement.Q("CollectText") as Label;
        counter = document.rootVisualElement.Q("Quantity") as Label;
        proximityWrapper = document.rootVisualElement.Q("ProximityWrapper");
        proximityIndicator = document.rootVisualElement.Q("ProximityValue") as Label;
    }

    private void OnEnable()
    {
        ArtifactIsCollectible.OnEventRaised += ShowCollectText;
        ArtifactNoLongerCollectible.OnEventRaised += HideCollectText;

        ArtifactCountUpdated.OnEventRaised += UpdateArtifactCounter;
        ArtifactProximityUpdated.OnEventRaised += UpdateProximity;

        DetectorEquipped.OnEventRaised += ToggleDetector;
    }

    private void OnDisable()
    {
        ArtifactIsCollectible.OnEventRaised -= ShowCollectText;
        ArtifactNoLongerCollectible.OnEventRaised -= HideCollectText;

        ArtifactCountUpdated.OnEventRaised -= UpdateArtifactCounter;
        ArtifactProximityUpdated.OnEventRaised -= UpdateProximity;

        DetectorEquipped.OnEventRaised -= ToggleDetector;
    }

    private void ShowCollectText()
    {
        collectTextTip.visible = true;
    }

    private void HideCollectText()
    {
        collectTextTip.visible = false;
    }

    private void UpdateArtifactCounter(int count) 
    {
        counter.text = count.ToString();
    }

    private void UpdateProximity(float p)
    {
        if (p == 0f)
        {
            proximityIndicator.text = "";
        }
        else
        {
            proximityIndicator.text = p.ToString();
        }
    }

    private void ToggleDetector(bool toggle)
    {
        proximityWrapper.visible = toggle;
    }
}
