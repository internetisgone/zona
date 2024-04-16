using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PDA : MonoBehaviour
{
    private UIDocument document;
    private VisualElement StalkerList;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        StalkerList = document.rootVisualElement.Q("StalkerList") as VisualElement;
    }

    // todo
    // display num of artifacts for each stalker 
}
