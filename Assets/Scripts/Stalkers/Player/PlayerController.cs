using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[SerializeField]
    //public bool useRigidbody = true;

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
    public float speed = 20f;
    public float jumpForce = 50f;
    public float gravityModifier = 2f;

    private bool isJumping;
    private bool isGrounded;
    private float ySpeed;

    private CharacterController controller; // temp 
    private Animator animator;

    //private bool lmbPressed = false; // temp
    //public GameObject projectile; // temp

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

        isJumping = false;
        isGrounded = true;
        ySpeed = 0;
    }

    void Update()
    {
        GetInput();

        UpdateMouseLook();

        Move();
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

    private void UpdateMouseLook()
    {
        // rotation along Y axis (yaw)
        rotationY -= mouseX;
        if (rotationY < -180) rotationY = 180f;
        else if (rotationY > 180) rotationY = -180f;

        // rotation along X axis (pitch)
        rotationX -= mouseY;
        rotationX = Math.Clamp(rotationX, -45, 45);

        // apply rotation
        transform.rotation = Quaternion.Euler(rotationX * mouseSensitivity, -rotationY * mouseSensitivity, 0f);
    }

    private void Move()
    {
        // movement based on character controller //

        Vector3 movementKeyboard = transform.right * horizontalInput + transform.forward * verticalInput;
        movementKeyboard.y = 0;

        // apply gravity
        ySpeed = - Physics.gravity.magnitude;

        if (isGrounded && isJumping)
        {
            ySpeed += jumpForce;
            isGrounded = false;
        }

        Vector3 movement = movementKeyboard + ySpeed * Vector3.up;

        controller.Move(movement * speed * Time.deltaTime);

        animator.SetFloat("Speed", new Vector2(movement.x, movement.z).magnitude * speed);
    }

    void FixedUpdate()
    {
        //if (lmbPressed)
        //{
        //    Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        //}

        //// movement based on rigidbody //

        //// keyboard input
        //Vector3 movementKeyboard = new Vector3(horizontalInput, 0f, verticalInput);

        //// move in the direction of mouse look
        //if (movementKeyboard.magnitude > 0)
        //{
        //    Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        //    rb.AddForce(movement * speed);
        //}

        //// jump
        //if (isGrounded && isJumping)
        //{
        //    //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //    rb.velocity = Vector3.up * jumpForce;
        //    isGrounded = false;
        //}
    }

    //// check for collision with ground
    //// does not work with char controller which has a built in capsule collider 
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Collision with " + collision.gameObject);
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    // check for collision with ground
    // works with char controller
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("Collision with " + hit.gameObject);
        if (hit.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
