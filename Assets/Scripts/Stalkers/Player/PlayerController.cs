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
    //private float curSpeed = 0;
    public float speed = 10f;
    //public float acceleration = 5f;
    public float jumpForce = 10f;
    public float gravityModifier = 2f;

    private bool isJumping;
    private bool isGrounded;
    private float ySpeed;

    // private CharacterController controller; // temp 
    private Animator animator;
    private GameObject firstPersonCamera; 

    //private bool lmbPressed = false; // temp
    //public GameObject projectile; // temp

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        firstPersonCamera = transform.GetChild(1).gameObject; // todo

        Physics.gravity *= gravityModifier;

        Cursor.lockState = CursorLockMode.Locked;

        isJumping = false;
        isGrounded = true;
        ySpeed = 0;
    }

    void Update()
    {
        GetInput();

        UpdateRotation();

        //Move();
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

        // rotation along X axis (pitch)
        rotationX -= mouseY;
        rotationX = Math.Clamp(rotationX, -45, 45);

        // turn 
        transform.rotation = Quaternion.Euler(0f, -rotationY * mouseSensitivity, 0f);

        // change camera pitch
        firstPersonCamera.transform.localRotation = Quaternion.Euler(rotationX * mouseSensitivity, 0f, 0f);
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

        Vector3 movement = movementKeyboard * speed;

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        animator.SetFloat("Speed", new Vector2(movement.x, movement.z).magnitude * speed);
    }

    void FixedUpdate()
    {
        Move();

        //if (lmbPressed)
        //{
        //    Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        //}
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
