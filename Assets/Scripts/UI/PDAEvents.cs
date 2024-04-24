using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PDAEvents : MonoBehaviour
{
    // ui 
    public VisualTreeAsset listItem;
    private UIDocument document;
    private VisualElement container;

    private VisualElement rankView;
    private VisualElement messageView;
    private Button rankTabBtn;
    private Button messageTabBtn;

    private VisualElement rankList;
    private bool isVisible = false;

    // data
    private List<CStalker> stalkersList;
    private int unknownStalkerCount = 3;

    private AudioSource audioSource;
    public AudioClip PDAOn;
    public AudioClip PDAOff;

    public EventVoid TogglePDAEvent;
    public EventStalkerInt StalkerStatsUpdated;

    void Awake()
    {
        document = GetComponent<UIDocument>();
        container = document.rootVisualElement.Q("Container");

        rankView = document.rootVisualElement.Q("RankView");
        messageView = document.rootVisualElement.Q("MessageView");
        rankTabBtn = document.rootVisualElement.Q<Button>("RankTabBtn");
        messageTabBtn = document.rootVisualElement.Q<Button>("MessageTabBtn");
        rankList = document.rootVisualElement.Q("List");

        container.visible = isVisible;
        rankView.SetEnabled(true);
        messageView.SetEnabled(false);

        // get list of stalkers including player 
        GameObject spawnerObj = GameObject.FindWithTag("SpawnerNPC");
        SpawnNPC spawner = spawnerObj?.GetComponent<SpawnNPC>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        CStalker player = playerObj?.GetComponent<CStalker>();

        if (spawner == null || player == null) return;

        stalkersList = spawner.stalkersList;
        stalkersList.Add(player);

        for (int i = 0; i < stalkersList.Count + unknownStalkerCount; i++)
        {
            VisualElement listItemContainer = listItem.Instantiate();
            rankList.Add(listItemContainer);
        }
        SetUnkonwnStalkerRank();

        audioSource = GetComponent<AudioSource>();

        //rankTabBtn.RegisterCallback<ClickEvent>(ShowRankTab);
        //messageTabBtn.RegisterCallback<ClickEvent>(ShowMessageTab);
    }

    private void OnEnable()
    {
        TogglePDAEvent.OnEventRaised += ToggleVisibility;
        StalkerStatsUpdated.OnEventRaised += OnStalkerDataUpdated;
    }

    private void OnDisable()
    {
        TogglePDAEvent.OnEventRaised -= ToggleVisibility;
        StalkerStatsUpdated.OnEventRaised += OnStalkerDataUpdated;
    }

    private void ToggleVisibility()
    {
        isVisible = !isVisible;

        if (isVisible)
        {
            audioSource.PlayOneShot(PDAOn);
            UpdateStalkerData();        
        }
        else
        {
            audioSource.PlayOneShot(PDAOff);
        }

        container.visible = isVisible;
    }

    private void OnStalkerDataUpdated(CStalker stalker, int num)
    {
        UpdateStalkerData();
    }

    private void UpdateStalkerData()
    {
        stalkersList.Sort();

        for (int i = 0; i < stalkersList.Count; i++)
        {
            VisualElement curItem = rankList.ElementAt(i + unknownStalkerCount);
            Label rank = curItem.Q<Label>("Rank");
            Label name = curItem.Q<Label>("Name");
            Label artifactCount = curItem.Q<Label>("Artifacts");

            CStalker curStalker = stalkersList[i];
            rank.text = (i + 1 + unknownStalkerCount).ToString();
            name.text = curStalker.Name;
            artifactCount.text = curStalker.ArtifactCount.ToString();
            //Debug.LogFormat("stalker guid {0}, name {1}", curStalker.Guid, curStalker.Name);
        }
    }

    //private void ShowRankTab(ClickEvent e)
    //{
    //    messageView.SetEnabled(false);
    //    rankView.SetEnabled(true);
    //}

    //private void ShowMessageTab(ClickEvent e)
    //{
    //    rankView.SetEnabled(false);
    //    messageView.SetEnabled(true);
    //}

    private void SetUnkonwnStalkerRank()
    {
        for (int i = 0; i < unknownStalkerCount; i++)
        {
            VisualElement curItem = rankList.ElementAt(i);
            Label rank = curItem.Q<Label>("Rank");
            rank.text = (i + 1).ToString();
        }
    }
}
