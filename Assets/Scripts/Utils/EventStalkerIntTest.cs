using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventStalkerInt))]

public class EventStalkerIntTest : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EventStalkerInt targetEvent = (EventStalkerInt)target;

        if (GUILayout.Button("Raise Event"))
        {
            targetEvent.RaiseEvent(new StalkerNPC(), 420);
        }
    }

}
