using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public EventBool CursorVisible;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        //DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        // cursor lock
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        else if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable()
    {
        CursorVisible.OnEventRaised += SetVisible;
    }

    private void OnDisable()
    {
        CursorVisible.OnEventRaised -= SetVisible;
    }
    public void SetVisible(bool visible)
    {
        Cursor.lockState = CursorLockMode.Confined;
        if (Cursor.visible == visible) return;
        Cursor.visible = visible;
    }
}
