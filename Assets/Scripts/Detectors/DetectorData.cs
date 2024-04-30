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

[CreateAssetMenu]

public class DetectorData : ScriptableObject
{
    public DetectorType DetectorType;
    public Sprite sprite;
    [Range(0.0f, 5.0f)]
    public float Interval;
    [Range(1, 20)]
    public int DetectionRange;
    [Range(1, 10)]
    public float VisibilityRange;
}