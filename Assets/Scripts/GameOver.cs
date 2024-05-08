using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public EventVoid GameOverEvent;
    public EventBool DetectorEquipped;
    public PlayerData PlayerData;

    private float purgeDelay = 1f;

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

        DetectorEquipped.RaiseEvent(false);

        // hide UI
        GameObject UI = GameObject.FindGameObjectWithTag("UI");
        if (UI != null) UI.SetActive(false);

        // update npc states
        GameObject spawnerObj = GameObject.FindWithTag("SpawnerNPC");
        if (spawnerObj != null)
        {
            SpawnNPC spawner = spawnerObj.GetComponent<SpawnNPC>();
            for (int i = 0; i < spawner.stalkersList.Count; i++)
            {
                if (spawner.stalkersList[i] is StalkerNPC)
                {
                    StalkerNPC stalker = spawner.stalkersList[i] as StalkerNPC;
                    stalker.ChangeState(StalkerNPC.PanicState);
                }
            }
        }

        // play cutscene
    }

    private IEnumerator PurgeAfterDelay()
    {
        yield return new WaitForSeconds(purgeDelay);
        Purge();
    }

    private void StartPurge()
    {
        StartCoroutine(PurgeAfterDelay());
    }
}
