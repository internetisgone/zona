using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offsetPos;
    // Start is called before the first frame update
    void Start()
    {
        offsetPos = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offsetPos;
    }
}
