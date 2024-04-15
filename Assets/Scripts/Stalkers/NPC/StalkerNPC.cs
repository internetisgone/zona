using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class StalkerNPC : CStalker
{
    private GameObject campfire; // temp
    private Vector3 goal; // temp

    private float rotSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;
    public StalkerState state;

    void Awake()
    {
        campfire = GameObject.FindWithTag("Campfire");
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        // temp
        state = StalkerState.Idle;
        goal = campfire.transform.position;
        InvokeRepeating("TrySetGoal", 5, 5); 
    }

    void FixedUpdate()
    {
        Vector3 movement = goal - transform.position;
        Vector3 forward = new Vector3(movement.x, 0, movement.z);

        if (forward.magnitude < 1)
        {
            // already at goal
            state = StalkerState.Idle;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            animator.SetFloat("Speed", 0);
            return;
        }

        // check if stalker is facing the goal
        bool shouldTurn = !IsParallel(forward.normalized, transform.forward);

        if (shouldTurn)
        {
            state = StalkerState.Turning;
            TurnTowards(forward);
        }
        else
        {
            state = StalkerState.Wandering;
            MoveStalker(new Vector3(movement.normalized.x, 0, movement.normalized.z));
            animator.SetFloat("Speed", new Vector2(rb.velocity.x, rb.velocity.z).magnitude);
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

    private void TrySetGoal()
    {
        if (state != StalkerState.Idle) return;

        // 20% chance of having a new goal
        bool hasNewGoal = UnityEngine.Random.value < 0.2f;
        if (!hasNewGoal) return;

        // new goal will be set after a random delay
        float delay = UnityEngine.Random.Range(1, 5);
        IEnumerator coroutine = SetRandomGoalWithDelay(delay);
        StartCoroutine(coroutine);
    }

    private IEnumerator SetRandomGoalWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int xRange = 50;
        int zRange = 50;

        float xCoord = UnityEngine.Random.Range(campfire.transform.position.x - xRange / 2, campfire.transform.position.x + xRange / 2);
        float zCoord = UnityEngine.Random.Range(campfire.transform.position.z - zRange / 2, campfire.transform.position.z + zRange / 2);

        goal = new Vector3(xCoord, transform.position.y, zCoord);
        Debug.LogFormat("New goal for {0}: {1}", Name, goal);
    }

    private void MoveStalker(Vector3 movementNormalized)
    {
        //transform.position += movementNormalized * Speed * Time.deltaTime;

        Vector3 curMovement = rb.velocity;
        Vector3 newMovement = Vector3.Lerp(curMovement, movementNormalized * Speed, 10f);

        rb.velocity = new Vector3(newMovement.x, rb.velocity.y, newMovement.z);
    }

    // todo prob move these functions to a util class
    private bool IsParallel(Vector2 xzMovement, Vector2 xzFacing)
    {
        float tolerance = 0.02f;
        return CloseEnough(xzMovement.x, xzFacing.x, tolerance) && CloseEnough(xzMovement.y, xzFacing.y, tolerance);
    }
    private bool CloseEnough(float x, float y, float tolerance)
    {
        return Math.Abs(x - y) <= tolerance;
    }

    private void TurnTowards(Vector3 forward)
    {
        Quaternion lookRotation = Quaternion.LookRotation(forward, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(Name + ": Get out of here stalker ");
        }
    }
}
