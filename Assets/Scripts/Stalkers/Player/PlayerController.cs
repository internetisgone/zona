using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // keyboard
    private float verticalInput;
    private float horizontalInput;
    // mouse
    public float mouseSensitivity = 1f;
    private float mouseX;
    private float mouseY;
    private float rotationX = 0;
    private float rotationY = 0;

    // movement
    Rigidbody rb;
    public float speed = 15f;
    [Range(0,1)]
    public float acceleration = 0.3f;
    public float jumpForce = 7f;
    public float gravityModifier = 3f;

    private bool isJumping;
    private bool isGrounded;

    private Animator animator;
    private GameObject firstPersonCamera; 
    private GameObject thirdPersonCamera;

    //private bool lmbPressed = false; // temp
    //public GameObject projectile; // temp

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        firstPersonCamera = transform.GetChild(1).gameObject; // todo
        thirdPersonCamera = transform.GetChild(2).gameObject; // todo

        Physics.gravity *= gravityModifier;

        Cursor.lockState = CursorLockMode.Locked;

        isJumping = false;
        isGrounded = true;
    }

    void Update()
    {
        GetInput();

        UpdateRotation();
    }

    void FixedUpdate()
    {
        Move();

        //if (lmbPressed)
        //{
        //    Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        //}
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    private void GetInput()
    {
        // keyboard
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyUp(KeyCode.Space)) isJumping = false;
        if (Input.GetKeyDown(KeyCode.Space)) isJumping = true;

        // mouse 
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        // temp
        //if (Input.GetMouseButtonDown(0))
        //{
        //    lmbPressed = true;
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    lmbPressed = false;
        //}
    }

    private void UpdateRotation()
    {
        // rotation along Y axis (yaw)
        rotationY -= mouseX;
        if (rotationY < -180) rotationY = 180f;
        else if (rotationY > 180) rotationY = -180f;

         // turn 
        transform.rotation = Quaternion.Euler(0f, -rotationY * mouseSensitivity, 0f);
    }

    private void Move()
    {
        Vector3 movementKeyboard = transform.right * horizontalInput + transform.forward * verticalInput;
        movementKeyboard.y = 0;

        if (isGrounded && isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        Vector3 curMovement = rb.velocity;
        Vector3 newMovement = Vector3.Lerp(curMovement, movementKeyboard * speed, acceleration);

        rb.velocity = new Vector3(newMovement.x, rb.velocity.y, newMovement.z);

        // Debug.LogFormat("movementKeyboard {0}, curMovement {1}, newMovement {2}, rb.velocity {3}", movementKeyboard, curMovement, newMovement, rb.velocity);

        animator.SetFloat("Speed", new Vector2(rb.velocity.x, rb.velocity.z).magnitude);
    }

    private void UpdateCamera()
    {
        // rotation along X axis (pitch)
        rotationX -= mouseY;
        float rotationX1 = Math.Clamp(rotationX, -45, 45);
        float rotationX2 = Math.Clamp(rotationX, -45, 10);

        firstPersonCamera.transform.localRotation = Quaternion.Euler(rotationX1 * mouseSensitivity, 0f, 0f);
        thirdPersonCamera.transform.localRotation = Quaternion.Euler(rotationX2 * mouseSensitivity, 0f, 0f);
    }

    // check for collision with ground
    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Collision with " + collision.gameObject);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // check for collision with ground (for char controller)
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    //Debug.Log("Collision with " + hit.gameObject);
    //    if (hit.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}
}
