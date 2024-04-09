using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : CStalker
{
    public EventInt ArtifactCountUpdated;

    public override void ChangeArtifactCount(int delta)
    {
        base.ChangeArtifactCount(delta);
        ArtifactCountUpdated.RaiseEvent(ArtifactCount);
    }
}
