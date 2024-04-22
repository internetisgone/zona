using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StalkerData / NPC")]
public class NPCData : ScriptableObject
{
    private static float DefaultSpeed = 1.8f;
    private static float DefaultTurnSpeed = 2f;
    private static float DefaultVisionRange = 3f;
    private static float DefaultCollectionRange = 2f;

    // movement
    [Range(1, 10f)]
    public float Speed = DefaultSpeed;
    public float TurnSpeed = DefaultTurnSpeed;

    // vision
    public float VisionRange = DefaultVisionRange;

    // artifact 
    public float CollectionRange = DefaultCollectionRange;

    public void Reset()
    {
        Speed = DefaultSpeed;
        TurnSpeed = DefaultTurnSpeed;
        VisionRange = DefaultVisionRange;
        CollectionRange = DefaultCollectionRange;
    }
}