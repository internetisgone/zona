using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnNPC : MonoBehaviour
{
    public GameObject stalkerNPC;
    private SpawnPoint[] spawnPoints;

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

    void Start()
    {
        // get a list of spawn points
        spawnPoints = GetSpawnPoints();

        SetBounds(stalkerNames.Length, spawnPoints.Length);

        // set a random count 
        count = UnityEngine.Random.Range(minCount, maxCount);
        
        // genrate random & unique indices for name and spawn points
        int[] randomIndices = GenerateRandomIndices(count, maxCount);

        // spawn new stalker NPCs
        for (int i = 0; i < count; i++)
        {
            int randomIndex = randomIndices[i]; 

            // instantiate
            GameObject stalkerObj = Instantiate(stalkerNPC, spawnPoints[randomIndex].transform.position, transform.rotation, spawnPoints[randomIndex].transform);
            StalkerNPC stalker = stalkerObj.GetComponent<StalkerNPC>();

            // set names
            stalker.Name = stalkerNames[randomIndex];
            Debug.Log("Spawned new stalker " + stalker.Name);
        }
    }

    private SpawnPoint[] GetSpawnPoints()
    { 
        return GetComponentsInChildren<SpawnPoint>(); ;
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
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, max);
                // todo Contains(0) always return true?
                // Debug.LogFormat("random num {0}, contained in array {1}", randomIndex, randomIndices.Contains(randomIndex));
            }
            while (i > 0 && randomIndices.Contains(randomIndex));

            randomIndices[i] = randomIndex;
        }
        return randomIndices;
    }
}
