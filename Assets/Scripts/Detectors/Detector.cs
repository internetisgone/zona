using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectorType
{
    Echo = 0,
    Bear = 1,
    Veles = 2,
    Svarog = 3
}
public class Detector : ScriptableObject
{
    public DetectorType DetectorType { get; set; }
    public float Interval { get; }
    public int Range { get; }

    // detector type is echo by default
    public Detector() : this(DetectorType.Echo)
    {
        
    }
    public Detector(DetectorType type)
    {
        DetectorType = type;

        switch (DetectorType)
        {
            case DetectorType.Echo:
                Interval = 5;
                Range = 5;
                break;
            case DetectorType.Bear:
                Interval = 3;
                Range = 8;
                break;
            case DetectorType.Veles:
                Interval = 2;
                Range = 10;
                break;
            case DetectorType.Svarog:
                Interval = 1;
                Range = 16;
                break;
        }
    }
}