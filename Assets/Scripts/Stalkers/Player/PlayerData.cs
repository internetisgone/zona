using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StalkerData / Player")]
public class PlayerData : ScriptableObject
{
    // movement
    public float Speed = 10f;
    public float SprintSpeed = 20f;

    // controls
    [Range(0.1f, 2f)]
    public float MouseSensitivity = 1f;
    public bool MovementEnabled = true;
    public bool DetectorEquipped = true;

    // artifact 
    public float CollectionRange = 2f;

    private void Reset()
    {
        MouseSensitivity = 1f;
        MovementEnabled = true;
        DetectorEquipped = true;
    }
}
