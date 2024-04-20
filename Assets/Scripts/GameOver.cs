using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public EventVoid GameOverEvent;
    public PlayerData PlayerData;

    private void OnEnable()
    {
        GameOverEvent.OnEventRaised += Purge;
    }

    private void OnDisable()
    {
        GameOverEvent.OnEventRaised += Purge;
    }

    private void Purge()
    {
        Debug.Log("gg");
        PlayerData.InputEnabled = false;
        // stop stalker movement
        // hide pda and detector
        // play cutscene
    }
}
