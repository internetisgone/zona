using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CStalker : MonoBehaviour, IComparable<CStalker>
{
    public string Name { get; set; }
    public int ArtifactCount { get; private set; }
    [SerializeField]
    public EventStalker StalkerCollectedArtifact;

    //public Guid Guid;
    public CStalker() : this("Marked One")
    {

    }

    public CStalker(string name)
    {
        //Guid = Guid.NewGuid();
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
        StalkerCollectedArtifact.RaiseEvent(this);
    }

    public virtual void Panic()
    {

    }

    //public void CheckIsOnSlope(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        Vector3 normal = collision.contacts[0].normal;
    //        Debug.DrawRay(transform.position, normal, Color.white, 1f);

    //        if (normal.normalized != Vector3.up)
    //        {
    //            IsOnSlope = true;
    //        }
    //        else
    //        {
    //            IsOnSlope = false;
    //        }
    //    }
    //}
}
