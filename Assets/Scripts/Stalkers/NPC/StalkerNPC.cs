using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Flags]
//public enum StalkerState
//{
//    Idle = 0,
//    Turning = 1 << 0,
//    Wandering = 1 << 1,
//    DetectedArtifact = 1 << 2,
//}

public enum StalkerState
{
    Idle = 0,
    Turning = 1,
    Wandering = 2,
    DetectedArtifact = 3,
}

public class StalkerNPC : CStalker
{
    public NPCData StalkerData { get; set; }
    public StalkerState State { get; private set; }
    public Vector3 GoalPosition { get; private set; }

    private GameObject campfire; // initial goal
    private GameObject goalArtifact;

    private Rigidbody rb;
    private Animator animator;

    private Vector3 slopeNormal;

    void Awake()
    {
        campfire = GameObject.FindWithTag("Campfire");
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        State = StalkerState.Idle;
        GoalPosition = campfire.transform.position;
        InvokeRepeating("TrySetGoal", 5, 5);
    }

    void FixedUpdate()
    {
        Vector3 movement = GoalPosition - transform.position;
        Vector3 forward = new Vector3(movement.x, 0, movement.z);

        if (forward.magnitude < 1)
        {
            // already at goal

            if (State == StalkerState.DetectedArtifact)
                CollectArtifact(goalArtifact);

            State = StalkerState.Idle;
            rb.velocity = Vector3.zero;
            animator.SetFloat("Speed", 0);
            return;
        }

        // check if stalker is facing the goal
        bool shouldTurn = !IsParallel(forward.normalized, transform.forward);

        if (shouldTurn)
        {
            // turn towards the goal
            //State |= StalkerState.Turning;
            State = StalkerState.Turning;
            rb.velocity = Vector3.zero;
            TurnTowards(forward);
        }
        else
        {
            // move to goal
            MoveStalker(movement.normalized * StalkerData.Speed);
            animator.SetFloat("Speed", new Vector2(rb.velocity.x, rb.velocity.z).magnitude);
            State = StalkerState.Wandering;

            if (State == StalkerState.DetectedArtifact && movement.magnitude < StalkerData.CollectionRange)
                CollectArtifact(goalArtifact);
        }
    }

    private void TrySetGoal()
    {
        if (State != StalkerState.Idle) return;

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

        int xRange = 70;
        int zRange = 40;

        float xCoord = UnityEngine.Random.Range(campfire.transform.position.x - xRange / 2, campfire.transform.position.x + xRange / 2);
        float zCoord = UnityEngine.Random.Range(campfire.transform.position.z - zRange / 2, campfire.transform.position.z + zRange / 2);

        SetGoal(new Vector3(xCoord, transform.position.y, zCoord));
    }

    private void MoveStalker(Vector3 targetMovement)
    {
        // check slope angle only when moving
        CheckGround();
        if (slopeNormal != Vector3.up)
        {
            targetMovement = Vector3.ProjectOnPlane(targetMovement, slopeNormal);

            Vector3 slidingForce = Vector3.ProjectOnPlane(rb.mass * Physics.gravity, slopeNormal);
            rb.AddForce(-slidingForce);
        }

        Vector3 movement = Vector3.Lerp(rb.velocity, targetMovement, 0.3f);

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    // temp
    private void CheckGround()
    {
        RaycastHit hit;
        float rayLength = 2f;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, 1 << 6))
        {
            slopeNormal = hit.normal;
            Debug.DrawRay(transform.position, slopeNormal, Color.white, 0.5f);
        }
    }

    public void SetGoal(Vector3 newGoal)
    {
        GoalPosition = new Vector3(newGoal.x, transform.position.y, newGoal.z);
        Debug.LogFormat("New goal for {0}: {1}", Name, GoalPosition);
    }

    public void TurnTowards(Vector3 forward)
    {
        Quaternion lookRotation = Quaternion.LookRotation(forward, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * StalkerData.TurnSpeed));
    }

    // artifact spotted, try move to it
    public void TryCollectArtifact(GameObject artifactObj)
    {
        State = StalkerState.DetectedArtifact;
        goalArtifact = artifactObj;
        SetGoal(artifactObj.transform.position);
    }

    public override void CollectArtifact(GameObject artifactObj)
    {
        base.CollectArtifact(goalArtifact);
        goalArtifact = null;
        // State = StalkerState.Idle;
        //State -= StalkerState.DetectedArtifact; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(Name + ": Get out of here stalker ");
        }
        //else if (collision.gameObject.CompareTag("Ground"))
        //{
        //    slopeNormal = collision.contacts[0].normal;
        //    Debug.DrawRay(transform.position, slopeNormal, Color.white, 1f);
        //}
    }

    // static util functions
    public static bool IsParallel(Vector2 xzMovement, Vector2 xzFacing)
    {
        float tolerance = 0.01f;
        return CloseEnough(xzMovement.x, xzFacing.x, tolerance) && CloseEnough(xzMovement.y, xzFacing.y, tolerance);
    }
    public static bool CloseEnough(float x, float y, float tolerance)
    {
        return Math.Abs(x - y) <= tolerance;
    }
}
