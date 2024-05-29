using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventsSO / Stalker")]
public class EventStalker : ScriptableObject
{
    public UnityAction<CStalker> OnEventRaised;

    public void RaiseEvent(CStalker stalker)
    {
        OnEventRaised?.Invoke(stalker);
    }
}
