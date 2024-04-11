using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Speed = 1.8f;
        //Inventory = new List<Artifact>();
        ArtifactCount = 0;
    }

    void Awake()
    {
    }

    public virtual void ChangeArtifactCount(int delta)
    {
        ArtifactCount += delta;
    }

    public void MoveStalker(Vector3 movementNormalized)
    {
        transform.position += movementNormalized * Speed * Time.deltaTime;
    }
}
