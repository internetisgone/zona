using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // keyboard
    private float verticalInput;
    private float horizontalInput;
    // mouse
    private float mouseSensitivity;
    private float mouseX;
    private float mouseY;
    private float rotationX = 0;
    private float rotationY = 0;

    // movement
    Rigidbody rb;
    private float speed;
    private float sprintSpeed;
    [Range(0,1)]
    public float acceleration = 0.1f;
    public float jumpForce = 7f;
    public float gravityModifier = 3f;

    private PlayerData playerData;

    private bool isJumping;
    private bool isGrounded;
    private bool isSprinting;

    private bool isOnSlope;
    private Vector3 slopeNormal;
    [Range (1,89)]
    private int maxSlopeAngle = 45;
    private int groundLayerMask = 1 << 6;

    private Animator animator;
    private GameObject firstPersonCamera; 
    private GameObject thirdPersonCamera;

    //private bool lmbPressed = false; // temp
    //public GameObject projectile; // temp

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        // get player data
        playerData = GetComponent<Player>().StalkerData;
        speed = playerData.Speed;
        sprintSpeed = playerData.SprintSpeed;
        mouseSensitivity = playerData.MouseSensitivity;

        firstPersonCamera = transform.GetChild(1).gameObject; // todo
        thirdPersonCamera = transform.GetChild(2).gameObject; // todo

        Physics.gravity *= gravityModifier;

        Cursor.lockState = CursorLockMode.Locked;

        isJumping = false;
        isGrounded = true;
        isSprinting = false;
        isOnSlope = false;
        slopeNormal = Vector3.up;
    }

    void Update()
    {
        GetInput();
        UpdateRotation();
    }

    void FixedUpdate()
    {
        CheckGround();
        Move();
        UpdateAnimationParams();
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }
    
    private void GetInput()
    {
        // keyboard
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyUp(KeyCode.Space)) isJumping = false;
        if (Input.GetKeyDown(KeyCode.Space)) isJumping = true;
        isSprinting = Input.GetKey(KeyCode.LeftShift);

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
        Vector3 movementInput = transform.right * horizontalInput + transform.forward * verticalInput;
        movementInput.y = 0;

        if (isGrounded && isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        Vector3 targetMovement = movementInput * (isSprinting ? sprintSpeed : speed);

        // todo handle slopes
        if (isOnSlope && isGrounded)
        {
            float slopeAngle = Vector3.Angle(slopeNormal, Vector3.up);
            if (slopeAngle < maxSlopeAngle)
            {
                targetMovement = Vector3.ProjectOnPlane(targetMovement, slopeNormal);

                // todo prevent sliding 
            }
            else if (slopeAngle < 90)
            {
                Debug.Log("slope too steep");
            }
        }

        Vector3 newMovement = Vector3.Lerp(rb.velocity, targetMovement, acceleration);

        rb.velocity = new Vector3(newMovement.x, rb.velocity.y, newMovement.z);

        // Debug.LogFormat("movementInput {0}, curMovement {1}, newMovement {2}, rb.velocity {3}", movementInput, curMovement, newMovement, rb.velocity);
        // 

    }

    private void UpdateAnimationParams()
    {
        animator.SetFloat("Speed", new Vector2(rb.velocity.x, rb.velocity.z).magnitude);
        animator.SetBool("IsSprinting", isSprinting && rb.velocity.magnitude > speed);
    }

    private void UpdateCamera()
    {
        // rotation along X axis (pitch)
        rotationX -= mouseY;
        float rotationX1 = Math.Clamp(rotationX, -45, 60);
        float rotationX2 = Math.Clamp(rotationX, -45, 10);

        firstPersonCamera.transform.localRotation = Quaternion.Euler(rotationX1 * mouseSensitivity, 0f, 0f);
        thirdPersonCamera.transform.localRotation = Quaternion.Euler(rotationX2 * mouseSensitivity, 0f, 0f);
    }

    private void CheckGround()
    {
        RaycastHit hit;
        float rayLength = 0.3f;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundLayerMask))
        {
            isGrounded = true;
            slopeNormal = hit.normal;
            Debug.DrawRay(transform.position, slopeNormal, Color.white, rayLength);
            if (slopeNormal != Vector3.up) isOnSlope = true;
            else isOnSlope = false;
        }
        else
        {
            isGrounded = false;
        }
    }

    //// check for collision with ground and calculate slope angle
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //Debug.LogFormat("Collision with {0}, normal {1}" + collision.gameObject, collision.);
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;

    //        Vector3 normal = collision.contacts[0].normal;
    //        slopeNormal = normal.normalized;
    //        //Debug.DrawRay(transform.position, normal, Color.white, 1f);
    //        if (slopeNormal != Vector3.up)
    //        {
    //            isOnSlope = true;
    //        }
    //        else
    //        {
    //            isOnSlope = false;
    //        }
    //    }

    //    //foreach (ContactPoint contact in collision.contacts)
    //    //{
    //    //    Debug.DrawRay(transform.position, contact.normal, Color.white, 1f);
    //    //    Vector3 temp = Vector3.Cross(contact.normal, Vector3.down);
    //    //    Vector3 groundSlopeDir = Vector3.Cross(temp, contact.normal);
    //    //    slopeAngle = Vector3.Angle(contact.normal, Vector3.up);
    //    //    Debug.LogFormat("contact point {0}, normal {1}, groundSlopeDir {2}, slopeAngle {3}", contact.point, contact.normal, groundSlopeDir, slopeAngle);
    //    //}
    //}
}
