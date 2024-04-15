using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class DetectArtifactsPlayer : DetectArtifacts
{
    public EventVoid ArtifactIsCollectible;
    public EventVoid ArtifactNoLongerCollectible;

    public EventFloat ArtifactProximityUpdated;

    GameObject firstPersonCamera;

    void Awake()
    {
        Owner = GetComponent<Player>();
        firstPersonCamera = transform.GetChild(1).gameObject;
        InvokeRepeating("Detect", 1f, Detector.Interval);

    }

    void Update()
    {
        if (IsDetected)
        {
            CheckIfCollectible();
        }
        else
        {
            // clear proximity indicator
            ArtifactProximityUpdated.RaiseEvent(0);

            //foreach (var artifact in collectible)
            //{
            //    if (artifact) artifact.UnsetHighlight();
            //}
            //collectible.Clear();
            //}
        }
    }

    // search for artifacts within the detector's range
    public override void Detect()
    {
        Collider[] artifactColliders = Physics.OverlapSphere(transform.position, Detector.Range, ArtifactLayerMask);
        if (artifactColliders.Length > 0)
        {
            Debug.LogFormat("{0} detected {1} artifacts using {2}", Owner.Name, artifactColliders.Length, Detector.DetectorType.ToString());
            IsDetected = true;
            float minDistance = Detector.Range;
            foreach (Collider collider in artifactColliders)
            {
                float distance = (transform.position - collider.transform.position).magnitude;
                if (distance < minDistance) minDistance = distance;
                // Debug.LogFormat("{0} is within {1} meters", collider.gameObject.name, distance); 
            }
            ArtifactProximityUpdated.RaiseEvent(minDistance);
        }
        else
        {
            IsDetected = false;
        }
    }

    // check if any of the detected artifacts is within arm's reach
    private void CheckIfCollectible()
    {
        Vector3 origin, direction;
        RaycastHit hit;

        origin = firstPersonCamera.transform.position;
        direction = firstPersonCamera.transform.forward;


        if (Physics.Raycast(origin, direction, out hit, CollectionRange, ArtifactLayerMask))
        {
            ArtifactIsCollectible.RaiseEvent();
            if (Input.GetKey(KeyCode.F))
            {
                CollectArtifact(hit.collider.gameObject);
                ArtifactProximityUpdated.RaiseEvent(0);
            }
        }
        else
        {
            ArtifactNoLongerCollectible.RaiseEvent();
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
