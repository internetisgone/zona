using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class DetectArtifactsNPC : DetectArtifacts
{
    private StalkerNPC Owner;
    public DetectorData DetectorData; // temp

    private float collectionRange;

    void Awake()
    {
        Owner = GetComponent<StalkerNPC>();
    }

    void Start()
    {
        collectionRange = Owner.StalkerData.CollectionRange;
        InvokeRepeating("Detect", 1f, DetectorData.Interval);
    }

    // search for artifacts within the detector's range
    public override void Detect()
    {
        Collider[] artifactColliders = Physics.OverlapSphere(transform.position, DetectorData.DetectionRange, ArtifactLayerMask);
        if (artifactColliders.Length > 0)
        {
            //Debug.LogFormat("{0} detected {1} artifacts using {2}", Owner.Name, artifactColliders.Length, Detector.DetectorType.ToString());
            IsDetected = true;
            float minDistance = DetectorData.DetectionRange;
            foreach (Collider collider in artifactColliders)
            {
                // todo get the nearest artifact

                float distance = (transform.position - collider.transform.position).magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
                // Debug.LogFormat("{0} is within {1} meters", collider.gameObject.name, distance); 



                // todo turn to the nearest artifact then collect

                if (distance <= collectionRange)
                {
                    Owner.CollectArtifact(collider.gameObject);
                }

            }
        }
        else
        {
            IsDetected = false;
        }
    }

    void OnDrawGizmos()
    {
        //// draw detection radius
        //if (Application.isPlaying)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawWireSphere(transform.position, Detector.Range);
        // }
    }
}
