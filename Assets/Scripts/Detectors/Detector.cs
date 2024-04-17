using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Detector : MonoBehaviour
{
    //protected DetectorData DetectorData;
    public DetectorType DetectorType { get; private set; }
    public bool isEquipped {  get; set; }
    public bool IsDetected { get; set; }
    public int ArtifactLayerMask { get; }

    public Detector()
    {
        DetectorType = DetectorType.Echo;
        isEquipped = true;
        IsDetected = false;

        //int artifactLayer = LayerMask.NameToLayer("Artifact");
        //ArtifactLayerMask = 1 << artifactLayer;
        ArtifactLayerMask = 1 << 7;
    }

    public abstract void Detect();

    public void UpdateDetectorType(DetectorType type)
    {
        DetectorType = type;
        // load new detector data
    }
}
