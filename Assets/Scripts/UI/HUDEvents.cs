using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDEvents : MonoBehaviour
{
    private UIDocument document;
    private Label collectTextTip;
    private Label counter;
    private VisualElement proximityWrapper;
    private Label proximityIndicator;
    private VisualElement NotificationContainer;
    public VisualTreeAsset Notification;

    public EventVoid ArtifactIsCollectible;
    public EventVoid ArtifactNoLongerCollectible;
    public EventFloat ArtifactProximityUpdated;

    public EventStalkerInt StalkerStatsUpdated;

    public EventVoid DetectorEquipped;
    public PlayerData PlayerData;

    private WaitForSeconds hideNotifDelay = new WaitForSeconds(2);

    void Awake()
    {
        document = GetComponent<UIDocument>();
        collectTextTip = document.rootVisualElement.Q("CollectText") as Label;
        counter = document.rootVisualElement.Q("Quantity") as Label;
        proximityWrapper = document.rootVisualElement.Q("ProximityWrapper");
        proximityIndicator = document.rootVisualElement.Q("ProximityValue") as Label;
        NotificationContainer = document.rootVisualElement.Q("PDANotifContainer");
    }

    private void OnEnable()
    {
        ArtifactIsCollectible.OnEventRaised += ShowCollectText;
        ArtifactNoLongerCollectible.OnEventRaised += HideCollectText;

        ArtifactProximityUpdated.OnEventRaised += UpdateProximity;

        DetectorEquipped.OnEventRaised += ToggleDetector;

        StalkerStatsUpdated.OnEventRaised += DisplayPdaNotification;
    }

    private void OnDisable()
    {
        ArtifactIsCollectible.OnEventRaised -= ShowCollectText;
        ArtifactNoLongerCollectible.OnEventRaised -= HideCollectText;

        ArtifactProximityUpdated.OnEventRaised -= UpdateProximity;

        DetectorEquipped.OnEventRaised -= ToggleDetector;

        StalkerStatsUpdated.OnEventRaised -= DisplayPdaNotification;
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
            proximityIndicator.text = p.ToString("0.00");
        }
    }

    private void ToggleDetector()
    {
        PlayerData.DetectorEquipped = !PlayerData.DetectorEquipped;
        proximityWrapper.visible = PlayerData.DetectorEquipped;
    }

    private void DisplayPdaNotification(CStalker stalker, int artifactCount)
    {
        if (stalker is Player)
        {
            UpdateArtifactCounter(stalker.ArtifactCount);
        }

        // todo recycle notif objects
        VisualElement notif = Notification.Instantiate();
        Label notifText = notif.Q<Label>("Notification");
        notifText.text = stalker.Name + " collected " + artifactCount + " artifact.";

        NotificationContainer.Add(notif);
        IEnumerator coroutine = HideNotifAfterDelay(notif);
        StartCoroutine(coroutine);
    }

    private IEnumerator HideNotifAfterDelay(VisualElement notif)
    {
        yield return hideNotifDelay;
        NotificationContainer.Remove(notif);
    }
}
