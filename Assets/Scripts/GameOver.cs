using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public EventVoid GameOverEvent;
    public PlayerData PlayerData;

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
        Debug.Log("gg");

        // disable player movement
        PlayerData.MovementEnabled = false;

        // hide UI
        GameObject UI = GameObject.FindGameObjectWithTag("UI");
        if (UI != null) UI.SetActive(false);

        // stop stalker movement / update state


        // play cutscene
    }

    private IEnumerator PurgeAfterDelay()
    {
        yield return new WaitForSeconds(1);
        Purge();
    }

    private void StartPurge()
    {
        StartCoroutine(PurgeAfterDelay());
    }
}
