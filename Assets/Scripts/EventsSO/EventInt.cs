using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventsSO / Int")]
public class EventInt : ScriptableObject
{
    public UnityAction<int> OnEventRaised;

    public void RaiseEvent(int arg)
    {
        OnEventRaised?.Invoke(arg);
    }
}
