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

    public EventBool ArtifactCollectible;
    public EventFloat ArtifactProximityUpdated;

    public EventStalkerInt StalkerStatsUpdated;

    public EventVoid DetectorEquipped;

    public PlayerData PlayerData;

    // audio
    private AudioSource audioSource;
    public AudioClip notificationSound;

    private WaitForSeconds hideNotifDelay = new WaitForSeconds(2);

    void Awake()
    {
        document = GetComponent<UIDocument>();
        collectTextTip = document.rootVisualElement.Q("CollectText") as Label;
        counter = document.rootVisualElement.Q("Quantity") as Label;
        proximityWrapper = document.rootVisualElement.Q("ProximityWrapper");
        proximityIndicator = document.rootVisualElement.Q("ProximityValue") as Label;
        NotificationContainer = document.rootVisualElement.Q("NotifContainer");

        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < NotificationContainer.childCount; i++)
        {
            NotificationContainer.ElementAt(i).visible = false;
        }
    }

    private void OnEnable()
    {
        ArtifactCollectible.OnEventRaised += ToggleCollectText;

        ArtifactProximityUpdated.OnEventRaised += UpdateProximity;

        DetectorEquipped.OnEventRaised += ToggleDetector;

        StalkerStatsUpdated.OnEventRaised += DisplayPdaNotification;
    }

    private void OnDisable()
    {
        ArtifactCollectible.OnEventRaised -= ToggleCollectText;

        ArtifactProximityUpdated.OnEventRaised -= UpdateProximity;

        DetectorEquipped.OnEventRaised -= ToggleDetector;

        StalkerStatsUpdated.OnEventRaised -= DisplayPdaNotification;
    }

    private void ToggleCollectText(bool visible)
    {
        collectTextTip.visible = visible;
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
        audioSource.PlayOneShot(notificationSound);

        if (stalker is Player)
        {
            UpdateArtifactCounter(stalker.ArtifactCount);
        }

        for (int i = 0; i < NotificationContainer.childCount; i++)
        {
            VisualElement notifWrapper = NotificationContainer.ElementAt(i);

            // activate the first hidden element
            if (notifWrapper.visible == false)
            {
                Label notifText = notifWrapper.Q<Label>("Text");
                notifText.text = stalker.Name + " collected " + artifactCount + " artifact.";

                notifWrapper.visible = true;
                IEnumerator coroutine = HideNotifAfterDelay(notifWrapper);
                StartCoroutine(coroutine);
                return;
            }
        }        
    }

    private IEnumerator HideNotifAfterDelay(VisualElement notif)
    {
        yield return hideNotifDelay;
        notif.visible = false;
    }
}
