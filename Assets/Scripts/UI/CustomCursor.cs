using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomCursor : MonoBehaviour
{
    public EventBool CursorVisible;
    public EventBool PauseMenuToggleEvent;
    public EventVoid GameOverEvent;

    private bool canPause;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canPause)
            {
                if (Time.timeScale == 0) // paused
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    PauseMenuToggleEvent.RaiseEvent(true);
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void OnEnable()
    {
        CursorVisible.OnEventRaised += SetVisible;
        GameOverEvent.OnEventRaised += OnGameOver;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        CursorVisible.OnEventRaised -= SetVisible;
        GameOverEvent.OnEventRaised -= OnGameOver;
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    public void SetVisible(bool visible)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = visible;
    }

    private void OnGameOver()
    {
        SetVisible(true);
        canPause = false;
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        if (next.name == "Zona")
        {
            SetVisible(false);
            canPause = true;
        }
        else if (next.name == "MenuScreen")
        {
            SetVisible(true);
            canPause = false;
        }
    }
}
