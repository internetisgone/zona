using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offsetPos;

    void Awake()
    {
        offsetPos = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offsetPos;
    }
}
