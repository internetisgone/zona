using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class DetectArtifacts : MonoBehaviour
{
    [HideInInspector]
    public Detector detector;
    private int artifactLayerMask;

    private bool isDetected;

    public EventVoid ArtifactIsCollectible;
    public EventVoid ArtifactNoLongerCollectible;

    public EventFloat ArtifactProximityUpdated;

    private CStalker owner;
    private bool isPlayer;

    // temp
    float collectionRange = 1.8f;
    GameObject firstPersonCamera;

    void Awake()
    {
        detector = ScriptableObject.CreateInstance<Detector>();
        isDetected = false;

        int artifactLayer = LayerMask.NameToLayer("Artifact");
        artifactLayerMask = 1 << artifactLayer;

        owner = GetComponent<CStalker>();
        isPlayer = owner is Player;
        if (isPlayer )
        {
            // todo
            firstPersonCamera = transform.GetChild(1).gameObject;
        }
    }
    void Start()
    {
        InvokeRepeating("Detect", 1f, detector.Interval);
    }

    void Update()
    {
        if (isDetected)
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
    void Detect()
    {
        Collider[] artifactColliders = Physics.OverlapSphere(transform.position, detector.Range, artifactLayerMask);
        if (artifactColliders.Length > 0)
        {
            Debug.LogFormat("{0} detected {1} artifacts using {2}", owner.Name, artifactColliders.Length, detector.DetectorType.ToString());
            isDetected = true;
            float minDistance = detector.Range;
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
            isDetected = false;
        }
    }

    // check if any of the detected artifacts is within arm's reach
    private void CheckIfCollectible()
    {
        RaycastHit hit;

        //Vector3 origin = transform.position + armOrigin * transform.up;
        Vector3 origin = firstPersonCamera.transform.position;

        if (Physics.Raycast(origin, firstPersonCamera.transform.forward, out hit, collectionRange, artifactLayerMask))
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

    private void CollectArtifact(GameObject artifactObj)
    {
        // Artifact artifact = artifactObj.GetComponent<Artifact>();
        // artifact.SetHighlight();
        owner.ChangeArtifactCount(1); // temp way to add to inventory 
        Destroy(artifactObj);
        ArtifactNoLongerCollectible.RaiseEvent();
    }

    void OnDrawGizmos()
    {
        //// draw detection radius
        //if (Application.isPlaying)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawWireSphere(transform.position, detector.Range);
        // }
    }
}
