using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerNPC : CStalker
{
    private GameObject campfire; // temp object that act as a goal
    private float rotSpeed = 3f;
    private Rigidbody rb;

    // todo use same animator for npc and player?
    private Animator animator;

    readonly static int IDLE = Animator.StringToHash("Base Layer.idle");
    readonly static int WALK = Animator.StringToHash("Base Layer.walk");
    readonly static int WALK_DRUNK = Animator.StringToHash("Base Layer.walk_drunk");

    private int curAnimation = IDLE;
    void Awake()
    {
        campfire = GameObject.FindWithTag("Campfire");
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 movement = campfire.transform.position - transform.position;

        // check if stalker is facing the goal
        Vector2 xzMovement = new Vector2(movement.x, movement.z);
        Vector2 xzFacing = new Vector2 (transform.forward.x, transform.forward.z);
        bool shouldTurn = ! IsParallel(xzMovement.normalized, xzFacing);

        if (shouldTurn)
        {
            TurnTowards(movement);
        }
        else
        {
            if (movement.magnitude > 3)
            {
                ChangeAnimation(WALK);
                MoveStalker(new Vector3(movement.normalized.x, 0, movement.normalized.z));
            }
            else
            {
                ChangeAnimation(IDLE);
            }
        }

        //if (rb.velocity.magnitude >= Speed)
        //{
        //    ChangeAnimation(WALK);
        //}
        //else
        //{
        //    ChangeAnimation(IDLE);
        //}
    }

    private void ChangeAnimation(int newAnimation, float crossfadeDuration = 0.2f)
    {
        if (curAnimation != newAnimation)
        {
            curAnimation = newAnimation;
            animator.CrossFade(curAnimation, crossfadeDuration);
        }
    }

    // todo prob move these functions to a util class
    private bool IsParallel(Vector2 xzMovement, Vector2 xzFacing)
    {
        float tolerance = 0.01f;
        return CloseEnough(xzMovement.x, xzFacing.x, tolerance) && CloseEnough(xzMovement.y, xzFacing.y, tolerance);
    }
    private bool CloseEnough(float x, float y, float tolerance)
    {
        return Math.Abs(x - y) <= tolerance;
    }

    private void TurnTowards(Vector3 movement)
    {
        Quaternion lookRotation = Quaternion.LookRotation(movement, Vector3.up);
        transform.localRotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
    }

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(Name + ": Get out of here stalker ");
        }
    }
}
