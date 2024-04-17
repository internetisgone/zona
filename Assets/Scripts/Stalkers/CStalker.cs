using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CStalker : MonoBehaviour, IComparable<CStalker>
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

    // stalkers can be sorted by artifact count
    public int CompareTo(CStalker stalker)
    {
        if (ArtifactCount.CompareTo(stalker.ArtifactCount) != 0)
            return -ArtifactCount.CompareTo(stalker.ArtifactCount);
        else 
            return Name.CompareTo(stalker.Name);
    }

    public virtual void CollectArtifact(GameObject artifactObj)
    {
        if (!artifactObj) return;
        Artifact artifact = artifactObj.GetComponent<Artifact>();
        if (!artifact) return;

        artifact.OnCollected();
        ArtifactCount += 1;
        Debug.LogFormat("{0} added 1 artifact to inventory", Name);
    }
}
