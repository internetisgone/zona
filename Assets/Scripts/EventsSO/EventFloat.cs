using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventsSO / Float")]
public class EventFloat : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void RaiseEvent(float arg)
    {
        OnEventRaised?.Invoke(arg);
    }
}
