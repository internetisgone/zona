using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StalkerData / NPC")]
public class NPCData : ScriptableObject
{
    // movement
    [Range(1, 10f)]
    public float Speed = 10f;
    public float TurnSpeed = 5f;

    // artifact 
    public float CollectionRange = 2f;

    public void Reset()
    {
        Speed = 1.8f;
        TurnSpeed = 2f;
        CollectionRange = 2f;
    }
}