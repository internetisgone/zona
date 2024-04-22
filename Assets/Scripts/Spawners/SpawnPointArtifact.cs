using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointArtifact : SpawnPoint
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 7);
    }
}