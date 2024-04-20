using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StalkerData / Player")]
public class PlayerData : ScriptableObject
{
    // movement
    public float Speed = 3f;
    public float SprintSpeed = 7f;

    // controls
    [Range(0.1f, 2f)]
    public float MouseSensitivity = 1f;
    public bool MovementEnabled = true;
    public bool DetectorEquipped = true;

    // artifact 
    public float CollectionRange = 2f;

    public void Reset()
    {
        Speed = 3f;
        SprintSpeed = 7f;
        MouseSensitivity = 1f;
        MovementEnabled = true;
        DetectorEquipped = true;
        CollectionRange = 2f;
    }
}
