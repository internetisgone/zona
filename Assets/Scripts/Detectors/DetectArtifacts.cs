using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public abstract class DetectArtifacts : MonoBehaviour
{
    public Detector Detector { get; set; }
    public bool IsDetected { get; set; }
    public int ArtifactLayerMask { get; }

    public DetectArtifacts()
    { 
        Detector = new Detector();
        IsDetected = false;

        //int artifactLayer = LayerMask.NameToLayer("Artifact");
        //ArtifactLayerMask = 1 << artifactLayer;
        ArtifactLayerMask = 1 << 7;
    }

    // search for artifacts within the detector's range
    public abstract void Detect();
}
