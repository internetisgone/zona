using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;


public class SpawnArtifacts : MonoBehaviour
{
    public GameObject artifactPrefab;

    // number of artifacts in a cluster
    private int minCount = 10;
    private int maxCount = 20;

    private float spawnRadius = 7f;
    private float minSpacing = 0.7f; // temp. min spacing = artifact diameter with some extra room

    void Start()
    {
        Artifact.TotalCount = 0;

        SpawnPointArtifact[] clusters = GetComponentsInChildren<SpawnPointArtifact>();

        // generate random spawn points inside each cluster
        for (int i = 0; i < clusters.Length; i++)
        {
            int count = Random.Range(minCount, maxCount + 1);
            List<Vector2> points = new List<Vector2>();

            //Debug.LogFormat("------ Generating {0} in cluster {1} ------", count, i);

            Vector3 center = clusters[i].transform.position;
            Vector2 xzCenter = new Vector2(center.x, center.z);
    
            for (int j = 0; j < count; j++)
            {
                Vector2 newPoint = GenerateNewPoint(xzCenter, spawnRadius);
                do
                {
                    newPoint = GenerateNewPoint(xzCenter, spawnRadius);
                }
                while (!IsValidPoint(newPoint, points));

                points.Add(newPoint);
            }

            //Debug.Log("----------------------------------");

            // instantiate artifacts
            for (int j = 0; j < points.Count; j++)
            {
                SpawnArtifact(points[j].x, points[j].y);
            }

            Artifact.TotalCount += count;
        }
    }

    // generate random points within a circle uniformly
    private Vector2 GenerateNewPoint(Vector2 center, float maxRadius)
    {
        // use square root to get a uniform distribution
        int rand = Random.Range(0, 100);
        float distance = maxRadius * Mathf.Sqrt(rand) / 10;

        // random angle 
        int angle = Random.Range(0, 360);
        float angleRad = Mathf.Deg2Rad * angle;

        float x = center.x + Mathf.Cos(angleRad) * distance;
        float y = center.y + Mathf.Sin(angleRad) * distance;

        //Debug.LogFormat("center {0}, distance {1}, angle {2}, x delta {3}, y delta {4}", center, distance, angle, Mathf.Cos(angleRad) * distance, Mathf.Sin(angleRad) * distance);

        return new Vector2(x, y);
    }

    private bool IsValidPoint(Vector2 newPoint, List<Vector2> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            float distance = (newPoint - points[i]).magnitude;
            if (distance < minSpacing) return false;
        }
        return true;
    }

    private void SpawnArtifact(float x, float z)
    {
        Instantiate(artifactPrefab, new Vector3(x, transform.position.y, z), transform.rotation, transform);

        //Resources.Load(artifact.PrefabPath);
        //artifactsList.Add(artifact);
    }
}
