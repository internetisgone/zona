using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo
public class RuntimeData : ScriptableObject
{
    public int ArtifactCount = 0;
    public List<CStalker> Stalkers;

    private void Awake()
    {
        SpawnNPC spawner = GameObject.FindWithTag("SpawnerNPC")?.GetComponent<SpawnNPC>();
    }
}