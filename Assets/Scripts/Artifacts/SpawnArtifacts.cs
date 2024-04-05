using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;


public class SpawnArtifacts : MonoBehaviour
{
    // private ArtifactData[] artifactData;
    public GameObject artifactPrefab;

    private int minCount = 10;
    private int maxCount = 20;
    private int xRange = 100;
    private int zRange = 100;

    void Start()
    {
        int count = Random.Range(minCount, maxCount);
        for (int i = 0; i < count; i++)
        {
            // todo use pre defined spawn points
            float xCoord = Random.Range(transform.position.x - xRange / 2, transform.position.x + xRange / 2);
            float zCoord = Random.Range(transform.position.z - zRange / 2, transform.position.z + zRange / 2);

            SpawnArtifact(xCoord, zCoord);
        }
    }

    private void SpawnArtifact(float x, float z)
    {
        Instantiate(artifactPrefab, new Vector3(x, transform.position.y, z), transform.rotation, transform);

        //Resources.Load(artifact.PrefabPath);
        //artifactsList.Add(artifact);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
