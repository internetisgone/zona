using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// temp
public enum StalkerState
{
    Idle = 0,
    Wandering = 1,
}
public abstract class CStalker : MonoBehaviour
{
    public string Name {  get; set; }
    //public DetectorType DetectorType;
    //public bool IsDetected { get; set; }
    public int ArtifactCount { get; private set; }
    public CStalker() : this("Marked One")
    {

    }

    public CStalker(string name)
    {
        Name = name;
        ArtifactCount = 0; 
    }

    //public abstract T GetStalkerData<T>() where T : ScriptableObject;

    public virtual void CollectArtifact(GameObject artifactObj)
    {
        ArtifactCount += 1;
        Destroy(artifactObj);
        Debug.LogFormat("{0} added 1 artifact to inventory", Name);
    }
}
