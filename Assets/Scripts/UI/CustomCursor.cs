using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public EventBool CursorVisible;
    public EventBool PauseMenuToggleEvent;

    private void Awake()
    {
        SetVisible(false);
        //DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // cursor lock
        // todo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuToggleEvent.RaiseEvent(true);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
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
        if (visible == true)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }
}
