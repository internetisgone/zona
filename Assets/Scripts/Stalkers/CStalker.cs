using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStalker : MonoBehaviour
{
    public string Name { get; set; }
    public float Speed { get; set; }
    public DetectorType DetectorType { get; set; }
    public List<Artifact> Inventory { get; set; }

    public CStalker() : this("Marked One")
    {
        
    }

    public CStalker(string name)
    {
        Name = name;
        Speed = 5f;
        Inventory = new List<Artifact>();
    }

    public void MoveStalker(Vector3 movementNormalized)
    {
        transform.position += movementNormalized * Speed * Time.deltaTime;
    }
}
