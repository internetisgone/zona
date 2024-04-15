using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

// temp
public enum StalkerState
{
    Idle = 0,
    Wandering = 1,
}
public class CStalker : MonoBehaviour
{
    public string Name { get; set; }
    public float Speed { get; set; }
    public DetectorType DetectorType { get; set; }
    // public List<Artifact> Inventory { get; set; }
    public int ArtifactCount { get; private set; }

    //// animation states
    //public readonly static int IDLE = Animator.StringToHash("Base Layer.idle");
    //public readonly static int WALK = Animator.StringToHash("Base Layer.walk");
    //public readonly static int WALK_DRUNK = Animator.StringToHash("Base Layer.walk_drunk");
    //public readonly static int RUN = Animator.StringToHash("Base Layer.run");

    public CStalker() : this("Marked One")
    {
        
    }

    public CStalker(string name)
    {
        Name = name;
        //Speed = 1.6f; // normal walking speed
        Speed = 5f; // temp
        //Inventory = new List<Artifact>();
        ArtifactCount = 0;
    }

    void Awake()
    {
    }

    public virtual void ChangeArtifactCount(int delta)
    {
        ArtifactCount += delta;
        Debug.LogFormat("{0} added {1} artifact(s) to inventory", Name, delta);
    }
}
