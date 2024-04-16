using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
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

    void Awake()
    {
        document = GetComponent<UIDocument>();
        container = document.rootVisualElement.Q("Container");
        uiList = document.rootVisualElement.Q("List");

        GameObject spawnerObj = GameObject.FindWithTag("SpawnerNPC");
        spawner = spawnerObj.GetComponent<SpawnNPC>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.GetComponent<CStalker>();

        container.visible = false;
    }

    void Start()
    {
        stalkersList = spawner.stalkersList;
        stalkersList.Add(player);

        for (int i = 0; i < stalkersList.Count; i++)
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

            for (int i = 0; i < stalkersList.Count; i++)
            {
                VisualElement curItem = uiList.ElementAt(i);
                CStalker curStalker = stalkersList[i];
                // Debug.LogFormat("stalker {0} has {1} artifacts, curItem {2}", curStalker.Name, curStalker.ArtifactCount, curItem);
                Label rank = curItem.Q<Label>("Rank");
                Label name = curItem.Q<Label>("Name");
                Label artifactCount = curItem.Q<Label>("Artifacts");

                rank.text = (i + 1).ToString();
                name.text = curStalker.Name;
                artifactCount.text = curStalker.ArtifactCount.ToString();
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
