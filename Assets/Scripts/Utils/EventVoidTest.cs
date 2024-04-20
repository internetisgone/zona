using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventVoid))]
public class EventVoidTest : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EventVoid targetEvent = (EventVoid)target;

        if (GUILayout.Button("Raise Event"))
        {
            targetEvent.RaiseEvent();
        }
    }
}
