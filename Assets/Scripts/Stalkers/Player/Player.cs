using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : CStalker
{
    public PlayerData StalkerData;
    private Animator animator;
    //public EventInt ArtifactCountUpdated;

    //public override void CollectArtifact(GameObject artifactObj)
    //{
    //    base.CollectArtifact(artifactObj);
    //    ArtifactCountUpdated.RaiseEvent(ArtifactCount);
    //}

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public override void Panic()
    {
        animator.SetTrigger("Panic");
    }
}
