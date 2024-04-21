using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
//public class RuntimeData : ScriptableObject
//{
//    public List<CStalker> Stalkers;

//    private void OnEnable()
//    {
//        GameObject spawnerObj = GameObject.FindWithTag("SpawnerNPC");
//        SpawnNPC spawner = spawnerObj?.GetComponent<SpawnNPC>();
//        GameObject playerObj = GameObject.FindWithTag("Player");
//        CStalker player = playerObj?.GetComponent<CStalker>();

//        if (spawner != null && player != null)
//        {
//            Stalkers = spawner.stalkersList;
//            Stalkers.Add(player);
//        }
//    }
//}