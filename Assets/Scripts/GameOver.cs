using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    public EventVoid GameOverEvent;
    public EventBool DetectorEquipped;
    public PlayerData PlayerData;

    private UIDocument document;
    private VisualElement container;
    private Button restartBtn;

    private float purgeDelay = 2f;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        container = document.rootVisualElement.Q("Container");
        restartBtn = document.rootVisualElement.Q<Button>("RestartBtn");
        restartBtn.RegisterCallback<ClickEvent>(OnRestart);
        container.visible = false;
    }

    private void OnEnable()
    {
        GameOverEvent.OnEventRaised += StartPurge;
    }

    private void OnDisable()
    {
        GameOverEvent.OnEventRaised += StartPurge;
    }

    private void Purge()
    {
        // disable player movement
        PlayerData.MovementEnabled = false;

        DetectorEquipped.RaiseEvent(false);

        // hide UI
        GameObject UI = GameObject.FindGameObjectWithTag("UI");
        if (UI != null) UI.SetActive(false);

        // stalkers panic
        GameObject spawnerObj = GameObject.FindWithTag("SpawnerNPC");
        if (spawnerObj != null)
        {
            SpawnNPC spawner = spawnerObj.GetComponent<SpawnNPC>();
            for (int i = 0; i < spawner.stalkersList.Count; i++)
            {
               spawner.stalkersList[i].Panic();
            }
        }

        // play cutscene

    }

    private IEnumerator PurgeCoroutine()
    {
        yield return new WaitForSeconds(purgeDelay);
        Purge();
        yield return new WaitForSeconds(2f); // temp
        container.visible = true;
    }

    private void StartPurge()
    {
        StartCoroutine(PurgeCoroutine());
    }

    private void OnRestart(ClickEvent e)
    {
        SceneManager.LoadScene("MenuScreen");
    }
}
