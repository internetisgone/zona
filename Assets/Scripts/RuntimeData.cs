using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RuntimeData : ScriptableObject
{
    public List<CStalker> Stalkers;

    private void OnEnable()
    {

    }

    private void Reset()
    {
        Stalkers.Clear();
    }
}