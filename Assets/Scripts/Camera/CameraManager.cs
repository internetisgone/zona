using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    public Camera FirstPerson;
    public Camera ThirdPerson;
    public Camera TopDown;

    private bool PlayStartupAnimation = false;

    private Animator animator;
    public EventBool EnableCamControl;

    void Awake()
    {
        animator = FirstPerson.GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        EnableFirstPersonCam();

        if (PlayStartupAnimation)
        {
            animator.SetTrigger("Shake");
            StartCoroutine("WaitForAnim");
        }
        else
        {
            animator.enabled = false;
            EnableCamControl.RaiseEvent(true);
        }

    }

    // temp ugly solution
    private IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(3f);
        EnableCamControl.RaiseEvent(true);
    }


# if UNITY_EDITOR

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) 
        {
            EnableFirstPersonCam();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            EnableThirdPersonCam();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            EnableTopDownCam();
        }
    }
    private void EnableThirdPersonCam()
    {
        FirstPerson.enabled = false;
        TopDown.enabled = false;
        ThirdPerson.enabled = true;
    }

    private void EnableTopDownCam()
    {
        ThirdPerson.enabled = false;
        FirstPerson.enabled = false;
        TopDown.enabled = true;
    }

# endif

    private void EnableFirstPersonCam()
    {
        ThirdPerson.enabled = false;
        TopDown.enabled = false;
        FirstPerson.enabled = true;
    }
}