using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StalkerData / Player")]
public class PlayerData : ScriptableObject
{
    // movement
    private static float DefaultSpeed = 3f;
    private static float DefaultSprintSpeed = 7f;

    // controls
    private static float DefaultMouseSensitivity = 1f;
    private static bool DefaultMovementEnabled = true;

    // artifact 
    private static bool DefaultDetectorEquipped = true;
    private static float DefaultCollectionRange = 2f;

    public float Speed = DefaultSpeed;
    public float SprintSpeed = DefaultSprintSpeed;

    [Range(0.1f, 2f)]
    public float MouseSensitivity = DefaultMouseSensitivity;
    public bool MovementEnabled = DefaultMovementEnabled;

    public bool DetectorEquipped = DefaultDetectorEquipped;
    public float CollectionRange = DefaultCollectionRange;

    public void Reset()
    {
        Speed = DefaultSpeed;
        SprintSpeed = DefaultSprintSpeed;
        MouseSensitivity = DefaultMouseSensitivity;
        MovementEnabled = DefaultMovementEnabled;
        DetectorEquipped = DefaultDetectorEquipped;
        CollectionRange = DefaultCollectionRange;
    }
}
