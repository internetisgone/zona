using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
public class DetectorNPC : Detector
{
    private StalkerNPC Owner;

    void Awake()
    {
        Owner = GetComponent<StalkerNPC>();
        SetDetectorType(DetectorType.Echo);
    }

    void Start()
    {
        InvokeRepeating("Detect", 1f, DetectorData.Interval);
    }

    // search for artifacts within the detector's range
    public override void Detect()
    {
        // temp 
        // ignore detection range 
        // for now, turn towards nearest artifact within visibility range 
        // and try collect it 

        // if (Owner.State == StalkerState.DetectedArtifact) return;

        Collider[] artifactColliders = Physics.OverlapSphere(transform.position, DetectorData.VisibilityRange, ArtifactLayerMask);
        if (artifactColliders.Length > 0)
        {
            //Debug.LogFormat("{0} detected {1} artifacts using {2}", Owner.Name, artifactColliders.Length, Detector.DetectorType.ToString());
            IsDetected = true;

            float minDistance = DetectorData.VisibilityRange; 
            GameObject nearestArtifact = null;

            foreach (Collider collider in artifactColliders)
            {
                // get the nearest artifact
                float distance = (transform.position - collider.transform.position).magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestArtifact = collider.gameObject;
                }
                // Debug.LogFormat("{0} is within {1} meters", collider.gameObject.name, distance); 
            }

            // try collect the nearest artifact
            if (minDistance <= Owner.StalkerData.CollectionRange)
            {
                if (nearestArtifact != null)
                {
                    nearestArtifact.GetComponent<Artifact>()?.ToggleVisibility(true);
                    Owner.SetArtifactAsGoal(nearestArtifact);
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
