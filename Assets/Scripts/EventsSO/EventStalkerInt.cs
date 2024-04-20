using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventsSO / Stalker Int")]
public class EventStalkerInt : ScriptableObject
{
    public UnityAction<CStalker, int> OnEventRaised;

    public void RaiseEvent(CStalker stalker, int arg)
    {
        OnEventRaised?.Invoke(stalker, arg);
    }
}
