using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public Camera camera3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) 
        {
            camera3.enabled = false;
            camera2.enabled = false;
            camera1.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            camera3.enabled = false;
            camera1.enabled = false;
            camera2.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            camera1.enabled = false;
            camera2.enabled = false;
            camera3.enabled = true; 
        }
    }

    public Camera GetMainCamera()
    {
        return camera1;
    }
}
