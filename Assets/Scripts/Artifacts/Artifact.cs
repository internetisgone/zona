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
    public static int TotalCount;
    [SerializeField]
    public EventVoid GameOverEvent;
    public ArtifactType Type { get; }
    public bool IsVisible {  get; private set; }
    private Animator animator;
    private new SkinnedMeshRenderer renderer;
    private Rigidbody rb;

    //public GameObject Prefab { get; private set; }
    //public bool isHighlighted = false;
    //private Color color;
    //private readonly static int FLOAT = Animator.StringToHash("Base Layer.Float");

    public Artifact() : this(ArtifactType.Moonlight)
    {

    }

    public Artifact(ArtifactType type)
    {
        Type = type;
        IsVisible = true; // temp. invisible by default
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>(); // temp
        renderer.enabled = IsVisible;
        //rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
    }

    public void ToggleVisibility(bool visible)
    {
        if (IsVisible == visible) return;

        IsVisible = visible;
        renderer.enabled = IsVisible;
        if (IsVisible == true)
        {
            PlayAnimation();
        }
    }

    public void OnCollected()
    {
        gameObject.SetActive(false);
        TotalCount--;
        if (TotalCount == 0)
        {
            // game over when all artifacts have been collected 
            GameOverEvent.RaiseEvent();
        }
    }

    public void PlayAnimation()
    {
        animator.SetTrigger("IsFloating");
    }

    //public IEnumerator PlayAnimation()
    //{
    //    float delay = 3f;

    //    // ToggleVisibility(true);
    //    animator.SetBool("IsFloating", true);

    //    yield return new WaitForSeconds(delay);

    //    if (animator)
    //    {
    //        animator.SetBool("IsFloating", false);
    //        // ToggleVisibility(false);
    //    } 
    //}

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}