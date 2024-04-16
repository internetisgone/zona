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
    public float MouseSensitivity = 1f;

    // artifact 
    public float CollectionRange = 2f;

}
