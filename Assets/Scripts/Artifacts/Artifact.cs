using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum ArtifactType
{
    Moonlight,
    Crystal,
    Fireball,
    StoneBlood,
    Sparkler
}

public class Artifact : MonoBehaviour
{
    public ArtifactType Type { get; }
    public GameObject Prefab { get; private set; }
    public bool isHighlighted = false;
    private Renderer renderer;
    private Color color;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        color = renderer.material.color;
    }

    // highlight when detected & ready to be collected (temp)
    public void SetHighlight()
    {
        isHighlighted = true;
        renderer.material.color = Color.white;
    }

    public void UnsetHighlight()
    {
        isHighlighted = false;
        renderer.material.color = color;
    }
    
    public void OnCollected(GameObject stalker)
    {
        // add to stalker inventory (send event?)
        // destroy self
    }
}

//[CreateAssetMenu]
//public class ArtifactData : ScriptableObject
//{
//    public ArtifactType ArtifactType;
//    public GameObject Prefab;
//}