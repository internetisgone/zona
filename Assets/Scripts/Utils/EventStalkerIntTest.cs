using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

# if UNITY_EDITOR

[CustomEditor(typeof(EventStalker))]

public class EventStalkerTest : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EventStalker targetEvent = (EventStalker)target;

        if (GUILayout.Button("Raise Event"))
        {
            targetEvent.RaiseEvent(new StalkerNPC());
        }
    }
}

# endif
