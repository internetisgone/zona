using System.Collections.Generic;
using UnityEngine;
public class DetectArtifacts : MonoBehaviour
{
    [HideInInspector]
    public Detector detector;
    private bool isDetected;
    private int artifactLayerMask;
    private List<Artifact> collectible;

    // temp
    float collectionRange = 1.8f;
    float armOrigin = 1.5f;

    void Awake()
    {
        detector = ScriptableObject.CreateInstance<Detector>();
        isDetected = false;
        collectible = new List<Artifact>();

        int artifactLayer = LayerMask.NameToLayer("Artifact");
        artifactLayerMask = 1 << artifactLayer;
    }
    void Start()
    {
        InvokeRepeating("Detect", 1f, detector.Interval);
    }

    void Update()
    {
        if (isDetected)
        {
            CheckIfWithinCollectionRange();
        }
        else
        {
            foreach (var artifact in collectible)
            {
                artifact.UnsetHighlight();
            }
            collectible.Clear();
        }
    }

    // search for artifacts within the detector's range
    void Detect()
    {
        Collider[] artifactColliders = Physics.OverlapSphere(transform.position, detector.Range, artifactLayerMask);
        if (artifactColliders.Length > 0)
        {
            Debug.LogFormat("{0} detected {1} artifacts using {2}", transform.gameObject.name, artifactColliders.Length, detector.DetectorType.ToString());
            isDetected = true;
            foreach (Collider collider in artifactColliders)
            {
                float distance = (transform.position - collider.transform.position).magnitude;
                Debug.LogFormat("{0} is within {1} meters", collider.gameObject.name, distance); 
            }
        }
        else
        {
            isDetected = false;
        }
    }

    // check if any of the detected artifacts is within arm's reach
    private void CheckIfWithinCollectionRange()
    {
        RaycastHit hit;

        Vector3 origin = transform.position + armOrigin * transform.up;

        if (Physics.Raycast(origin, transform.forward, out hit, collectionRange, artifactLayerMask))
        {
            Debug.LogFormat("Artifact can be collected " + hit.collider.gameObject);
            Artifact artifact = hit.collider.gameObject.GetComponent<Artifact>();
            artifact.SetHighlight();
            collectible.Add(artifact);

            // todo display smol hud
            // todo destroy gameobj
            // todo add to inventory
        }
        //if (Physics.SphereCast(origin, armWingspan / 2, transform.forward, out hit, armWingspan / 2))
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
