using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
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
    private Animator animator;

    private Renderer renderer;

    //public GameObject Prefab { get; private set; }
    //public bool isHighlighted = false;
    //private Color color;
    public Artifact() : this(ArtifactType.Moonlight)
    {

    }

    public Artifact(ArtifactType type)
    {
        Type = type;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();

        //renderer.enabled = false;
    }

    public void ToggleVisibility(bool visible)
    {
        if (renderer.enabled == visible) return;
        renderer.enabled = visible;
    }

    public void OnCollected()
    {
        Destroy(gameObject);
    }

    public IEnumerator PlayAnimation()
    {
        float delay = 3f;

        // ToggleVisibility(true);
        animator.SetBool("IsFloating", true);

        yield return new WaitForSeconds(delay);

        if (animator)
        {
            animator.SetBool("IsFloating", false);
            // ToggleVisibility(false);
        } 
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}