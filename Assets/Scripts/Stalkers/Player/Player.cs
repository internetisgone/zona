using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : CStalker
{
    // private List<Artifact> Inventory;
    private int artifactsCollected;

    void Awake()
    {
        artifactsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // press f to sphere cast and collect artifact in front of u
 
}
