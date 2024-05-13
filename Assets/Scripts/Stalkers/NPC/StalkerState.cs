using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerState
{
    public string StateName { get; private set; }
    public StalkerState()
    {
        StateName = this.GetType().Name; 
    }

    public virtual void OnEnter(StalkerNPC stalker) 
    { 
        Debug.LogFormat("{0} is entering {1}", stalker.Name, StateName);
    }
    public virtual void OnUpdate(StalkerNPC stalker) { }
    public virtual void OnExit(StalkerNPC stalker) 
    {
        Debug.LogFormat("{0} is exiting {1}", stalker.Name, StateName);
    }
}

public class BoredState : StalkerState
{
    public override void OnEnter(StalkerNPC stalker)
    {
        base.OnEnter(stalker);
        stalker.StartSettingRandomGoal();
        stalker.StartLookForArtifacts();
    }

    public override void OnUpdate(StalkerNPC stalker)
    {
        base.OnUpdate(stalker);

        if (stalker.IsAtGoalPosition())
        {
            stalker.KeepStill();
        }
        else
        {
            stalker.MoveToGoal();
        }
    }

    public override void OnExit(StalkerNPC stalker)
    {
        base.OnExit(stalker);
        stalker.StopSettingRandomGoal();
        stalker.StopLookingForArtifacts();
    }
}

public class ArtifactState : StalkerState
{
    public override void OnEnter(StalkerNPC stalker)
    {
        base.OnEnter(stalker);
    }

    public override void OnUpdate(StalkerNPC stalker)
    {
        base.OnUpdate(stalker);

        if (stalker.IsAtGoalPosition())
        {
            stalker.CollectGoalArtifact();
            stalker.ChangeState(StalkerNPC.BoredState);
        }
        else
        {
            stalker.MoveToGoal();
        }
    }

}

public class ChillingState : StalkerState
{
    public override void OnEnter(StalkerNPC stalker)
    {
        base.OnEnter(stalker);
        // sit down at campfire and relax
        // put detector away
    }

    public override void OnUpdate(StalkerNPC stalker)
    {
        base.OnUpdate(stalker);
        // very smol chance to leave current state
    }
}

public class PanicState : StalkerState
{
    public override void OnEnter(StalkerNPC stalker)
    {
        base.OnEnter(stalker);
        stalker.PlayPanicAnim();
    }

    public override void OnUpdate(StalkerNPC stalker)
    {
        base.OnUpdate(stalker);
        stalker.KeepStill();
    }
}


