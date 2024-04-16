using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventsSO / Bool")]
public class EventBool : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool arg)
    {
        OnEventRaised?.Invoke(arg);
    }
}
