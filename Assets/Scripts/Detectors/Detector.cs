using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Detector : MonoBehaviour
{
    public DetectorType DetectorType { get; private set; }
    public DetectorData DetectorData { get; private set; }

    public DetectorData Echo;
    public DetectorData Bear;
    public DetectorData Veles;
    public DetectorData Svarog;

    public bool IsDetected { get; set; }
    public static int ArtifactLayerMask = 1 << 7;

    public Detector()
    {
        //DetectorType = DetectorType.Echo;
        //DetectorData = Echo;
        IsDetected = false;
    }

    public abstract void Detect();

    public void SetDetectorType(DetectorType type)
    {
        DetectorType = type;
        
        switch (type)
        {
            case DetectorType.Echo:
                DetectorData = Echo; 
                break;
            case DetectorType.Bear:
                DetectorData = Bear;
                break;
            case DetectorType.Veles:
                DetectorData = Veles;   
                break;
            case DetectorType.Svarog:
                DetectorData = Svarog;
                break;
        }
    }
}
