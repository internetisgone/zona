using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public abstract class DetectArtifacts : MonoBehaviour
{
    public Detector Detector { get; set; }
    public bool IsDetected { get; set; }
    public int ArtifactLayerMask { get; }

    public CStalker Owner { get; set; }
    public float CollectionRange { get; set; }

    public DetectArtifacts()
    { 
        Detector = new Detector();
        IsDetected = false;

        //int artifactLayer = LayerMask.NameToLayer("Artifact");
        //ArtifactLayerMask = 1 << artifactLayer;
        ArtifactLayerMask = 1 << 7;

        CollectionRange = 2f;
    }

    // search for artifacts within the detector's range
    public abstract void Detect();

    public void CollectArtifact(GameObject artifactObj)
    {
        // Artifact artifact = artifactObj.GetComponent<Artifact>();
        // artifact.SetHighlight();
        Owner.ChangeArtifactCount(1); // temp way to add to inventory 
        Destroy(artifactObj);
    }
}
