using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnNPC : MonoBehaviour
{
    public GameObject stalkerNPC;
    public NPCData npcData;
    private SpawnPointNPC[] spawnPoints;

    public List<CStalker> stalkersList;

    private int count;
    private int minCount = 3;
    private int maxCount = 7;

    private string[] stalkerNames = {
        "Sid",
        "Wolf",
        "Nimble",
        "Fox",
        "Ghost",
        "Fang",
        "Scar",
        "Charon",
        };

    void Awake()
    {
        // get a list of spawn points
        spawnPoints = GetSpawnPoints();

        SetBounds(stalkerNames.Length, spawnPoints.Length);

        // set a random count 
        count = UnityEngine.Random.Range(minCount, maxCount + 1);
        
        // genrate random & unique indices for name and spawn points
        int[] randomIndices = GenerateRandomIndices(count, maxCount);

        // spawn new stalker NPCs
        for (int i = 0; i < count; i++)
        {
            int randomIndex = randomIndices[i]; 

            // instantiate
            GameObject stalkerObj = Instantiate(stalkerNPC, spawnPoints[randomIndex].transform.position, transform.rotation, spawnPoints[randomIndex].transform);
            StalkerNPC stalker = stalkerObj.GetComponent<StalkerNPC>();

            // init stalker data
            stalker.Name = stalkerNames[randomIndex];
            stalker.StalkerData = npcData;
            stalkersList.Add(stalker);
            Debug.Log("Spawned new stalker " + stalker.Name);
        }
    }

    private SpawnPointNPC[] GetSpawnPoints()
    { 
        return GetComponentsInChildren<SpawnPointNPC>(); ;
    }

    private void SetBounds(int namesLength, int spawnPointsLength)
    {
        // max shoud be <= names len and spawn points len
        // min should be <= all other 3

        if (maxCount > namesLength) maxCount = namesLength;
        if (maxCount > spawnPointsLength) maxCount = spawnPointsLength;
        if (minCount > maxCount) minCount = maxCount;    
    }

    public int[] GenerateRandomIndices(int length, int max)
    {
        int[] randomIndices = new int[length];
        for (int i = 0; i < length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, max + 1);
            do
            {
                randomIndex = UnityEngine.Random.Range(0, max + 1);
                // todo Contains(0) always return true?
                // Debug.LogFormat("random num {0}, contained in array {1}", randomIndex, randomIndices.Contains(randomIndex));
            }
            while (i > 0 && randomIndices.Contains(randomIndex));

            randomIndices[i] = randomIndex;
        }
        return randomIndices;
    }
}
