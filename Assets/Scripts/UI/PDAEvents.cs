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
    private SpawnNPC spawner;
    private List<CStalker> stalkersList;
    private CStalker player;
    private int unknownStalkerCount = 3;

    void Awake()
    {
        document = GetComponent<UIDocument>();
        container = document.rootVisualElement.Q("Container");
        uiList = document.rootVisualElement.Q("List");

        GameObject spawnerObj = GameObject.FindWithTag("SpawnerNPC");
        spawner = spawnerObj?.GetComponent<SpawnNPC>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj?.GetComponent<CStalker>();

        container.visible = false;
    }

    void Start()
    {
        if (spawner == null || player == null) return;

        stalkersList = spawner.stalkersList;
        stalkersList.Add(player);

        for (int i = 0; i < stalkersList.Count + unknownStalkerCount; i++)
        {
            VisualElement listItemContainer = listItem.Instantiate();
            uiList.Add(listItemContainer);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetVisible(!isVisible);
        }
    }

    private void SetVisible(bool visible)
    {
        if (isVisible == visible) return;

        isVisible = visible;

        if (isVisible)
        {
            stalkersList.Sort();

            for (int i = 0; i < stalkersList.Count + unknownStalkerCount; i++)
            {
                VisualElement curItem = uiList.ElementAt(i);
                Label rank = curItem.Q<Label>("Rank");
                Label name = curItem.Q<Label>("Name");
                Label artifactCount = curItem.Q<Label>("Artifacts");

                if (i < unknownStalkerCount)
                {
                    rank.text = (i + 1).ToString();
                    name.text = "???";
                    artifactCount.text = "???";
                }
                else
                {
                    CStalker curStalker = stalkersList[i - unknownStalkerCount];
                    rank.text = (i + 1).ToString();
                    name.text = curStalker.Name;
                    artifactCount.text = curStalker.ArtifactCount.ToString();
                }
            }

            // todo subscribe to events and update stats in real time?
        }
        else
        {
            // unsub
        }

        container.visible = visible;
    }
}
