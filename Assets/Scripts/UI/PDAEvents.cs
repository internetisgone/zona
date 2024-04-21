using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PDAEvents : MonoBehaviour
{
    // ui 
    public VisualTreeAsset listItem;
    private UIDocument document;
    private VisualElement container;
    private VisualElement uiList;
    private bool isVisible = false;

    // data
    private List<CStalker> stalkersList;
    private int unknownStalkerCount = 3;

    public EventVoid TogglePDAEvent;

    void Awake()
    {
        document = GetComponent<UIDocument>();
        container = document.rootVisualElement.Q("Container");
        uiList = document.rootVisualElement.Q("List");

        container.visible = isVisible;

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
            uiList.Add(listItemContainer);
        }
        SetUnkonwnStalkerRank();
    }

    private void OnEnable()
    {
        TogglePDAEvent.OnEventRaised += ToggleVisibility;
    }

    private void OnDisable()
    {
        TogglePDAEvent.OnEventRaised -= ToggleVisibility;
    }

    private void ToggleVisibility()
    {
        isVisible = !isVisible;

        if (isVisible)
        {
            stalkersList.Sort();

            for (int i = 0; i < stalkersList.Count; i++)
            {
                VisualElement curItem = uiList.ElementAt(i + unknownStalkerCount);
                Label rank = curItem.Q<Label>("Rank");
                Label name = curItem.Q<Label>("Name");
                Label artifactCount = curItem.Q<Label>("Artifacts");

                CStalker curStalker = stalkersList[i];
                rank.text = (i + 1 + unknownStalkerCount).ToString();
                name.text = curStalker.Name;
                artifactCount.text = curStalker.ArtifactCount.ToString();
                //Debug.LogFormat("stalker guid {0}, name {1}", curStalker.Guid, curStalker.Name);
            }

            // todo subscribe to events and update stats in real time?
        }
        else
        {
            // unsub
        }

        container.visible = isVisible;
    }

    private void SetUnkonwnStalkerRank()
    {
        for (int i = 0; i < unknownStalkerCount; i++)
        {
            VisualElement curItem = uiList.ElementAt(i);
            Label rank = curItem.Q<Label>("Rank");
            rank.text = (i + 1).ToString();
        }
    }
}
