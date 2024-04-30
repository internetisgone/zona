using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
public class DetectorPlayer : Detector
{
    private Player Owner;
    private float collectionRange;

    //public EventVoid ArtifactIsCollectible;
    //public EventVoid ArtifactNoLongerCollectible;
    public EventBool ArtifactIsCollectible;
    public EventFloat ArtifactProximityUpdated;
  
    GameObject firstPersonCamera;

    void Awake()
    {
        Owner = GetComponent<Player>();
        SetDetectorType(DetectorType.Echo);
    }

    void Start()
    {
        collectionRange = Owner.StalkerData.CollectionRange;
        firstPersonCamera = transform.GetChild(1).gameObject;
        InvokeRepeating("Detect", 1f, DetectorData.Interval);
    }

    void Update()
    {
        if (Owner.StalkerData.DetectorEquipped == false) return;

        if (IsDetected)
        {
            CheckIfCollectible();
        }
    }

    // search for artifacts within the detector's range
    public override void Detect()
    {
        Collider[] artifactColliders = Physics.OverlapSphere(transform.position, DetectorData.DetectionRange, ArtifactLayerMask);
        if (artifactColliders.Length > 0)
        {
            Debug.LogFormat("{0} detected {1} artifacts using {2}", Owner.Name, artifactColliders.Length, DetectorData.DetectorType.ToString());
            IsDetected = true;
            float minDistance = DetectorData.DetectionRange;
            foreach (Collider collider in artifactColliders)
            {
                float distance = (transform.position - collider.transform.position).magnitude;

                // get shortest distance
                if (distance < minDistance) minDistance = distance;

                // check if within visibility range
                if (distance < DetectorData.VisibilityRange)
                {
                    Artifact artifact = collider.gameObject.GetComponent<Artifact>();
                    artifact.ToggleVisibility(true);
                }

                // Debug.LogFormat("{0} is within {1} meters", collider.gameObject.name, distance); 
            }
            ArtifactProximityUpdated.RaiseEvent(minDistance);
        }
        else
        {
            IsDetected = false;
            ClearProximityDisplay();
        }
    }

    // check if any of the detected artifacts is within arm's reach
    private void CheckIfCollectible()
    {
        Vector3 origin, direction;
        RaycastHit hit;

        origin = firstPersonCamera.transform.position;
        direction = firstPersonCamera.transform.forward;

        if (Physics.Raycast(origin, direction, out hit, collectionRange, ArtifactLayerMask))
        // todo use non alloc
        {
            ArtifactIsCollectible.RaiseEvent(true);
            GameObject artifact = hit.collider.gameObject;
            //StartCoroutine(artifact.GetComponent<Artifact>().PlayAnimation());
            artifact.GetComponent<Artifact>().PlayAnimation();
            if (Input.GetKey(KeyCode.F) && Owner.StalkerData.MovementEnabled)
            {
                Owner.CollectArtifact(artifact);
                ArtifactProximityUpdated.RaiseEvent(0);
            }
        }
        else
        {
            ArtifactIsCollectible.RaiseEvent(false);
        }
    }

    private void ClearProximityDisplay()
    {
        ArtifactProximityUpdated.RaiseEvent(0);
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
