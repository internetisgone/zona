using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointNPC : SpawnPoint
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
