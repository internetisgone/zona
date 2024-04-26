using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Flags]
//public enum StalkerState
//{
//    Idle = 0,
//    Turning = 1 << 0,
//    Moving = 1 << 1,
//    DetectedArtifact = 1 << 2,
//}

public enum StalkerState
{
    Bored, DetectedArtifact, Chilling, Panic
}

public enum MovementState
{
    Idle, Turning, Moving
}

public class StalkerNPC : CStalker
{
    public NPCData StalkerData;
    public static int GroundLayerMask = 1 << 6;
    public static int ArtifactLayerMask = 1 << 7;
    private static int ObstacleLayerMask = 1 << 8;

    public StalkerState State { get; set; }

    //public static BoredState BoredState;
    //public static ArtifactState ArtifactState;
    //public static PanicState PanicState;

    //public StalkerState currentState = BoredState;

    public MovementState movementState;
    public Vector3 GoalPosition;
    private Vector3 movement;
    private Vector3 forward;
    private bool isSettingGoal;

    private static GameObject campfire; // initial goal
    private GameObject goalArtifact;

    private Rigidbody rb;
    private Animator animator;

    private bool isOnSlope;
    private Vector3 slopeNormal;


    void Awake()
    {
        campfire = GameObject.FindWithTag("Campfire");
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        State = StalkerState.Bored;
        movementState = MovementState.Idle;

        GoalPosition = campfire.transform.position;
        InvokeRepeating("TrySetGoal", 5, 5);

        // check vision frustum
        InvokeRepeating("LookForArtifacts", 1, StalkerData.LookInterval);

        isOnSlope = false;
        slopeNormal = Vector3.up;
    }

    void FixedUpdate()
    {
        UpdateStates();

        CheckGround();

        if (isOnSlope)
        {
            Vector3 slidingForce = Vector3.ProjectOnPlane(rb.mass * Physics.gravity, slopeNormal);
            rb.AddForce(-slidingForce);
        }

        if (movementState == MovementState.Idle)
        {
            rb.velocity = Vector3.zero;
            animator.SetFloat("Speed", 0);
        }
        else if (movementState == MovementState.Turning)
        {
            TurnTowards(forward);
            rb.velocity = Vector3.zero;
        }
        else if (movementState == MovementState.Moving)
        {
            MoveStalker(movement.normalized * StalkerData.Speed);
            animator.SetFloat("Speed", new Vector2(rb.velocity.x, rb.velocity.z).magnitude);
        }
    }

    private void UpdateStates()
    {
        // temp. panic has the highest priority
        if (State == StalkerState.Panic)
        {
            movementState = MovementState.Idle;
            animator.SetTrigger("Panic");
            return;
        }

        movement = GoalPosition - transform.position;
        forward = new Vector3(movement.x, 0, movement.z);

        if (forward.magnitude < 2)
        {
            // already at goal
            if (State == StalkerState.DetectedArtifact)
            {
                CollectArtifact(goalArtifact);
            }
            movementState = MovementState.Idle;
        }
        else
        {
            // check if stalker is facing the goal
            bool shouldTurn = !IsParallelxz(forward.normalized, transform.forward);

            if (shouldTurn)
            {
                // turn to goal
                movementState = MovementState.Turning;
                Debug.DrawRay(transform.position, transform.forward * 2, Color.white, 1f);
            }
            else
            {
                // move to goal
                movementState = MovementState.Moving;
                Debug.DrawRay(transform.position, transform.forward * 2, Color.blue, 1f);
            }
        }
        //Debug.LogFormat("{0} is {1}", Name, State);
    }


    // --****========== movement ==========***-- //

    private void MoveStalker(Vector3 targetMovement)
    {
        if (isOnSlope)
        {
            targetMovement = Vector3.ProjectOnPlane(targetMovement, slopeNormal);
        }

        // targetMovement = Vector3.Lerp(rb.velocity, targetMovement, 0.3f);

        rb.velocity = new Vector3(targetMovement.x, rb.velocity.y, targetMovement.z);
    }

    public void TurnTowards(Vector3 forward)
    {
        Quaternion lookRotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * StalkerData.TurnSpeed));
    }

    private void CheckGround()
    {
        RaycastHit hit;
        float rayLength = 2f;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, GroundLayerMask))
        {
            slopeNormal = hit.normal;
            //Debug.DrawRay(transform.position, slopeNormal, Color.white, 0.5f);
            if (slopeNormal != Vector3.up) isOnSlope = true;
            else isOnSlope = false;
        }
    }

    // --****========== random goal ==========***-- //

    private void TrySetGoal()
    {
        if (State != StalkerState.Bored || isSettingGoal) return;

        // 20% chance of having a new goal
        bool hasNewGoal = UnityEngine.Random.value < 0.2f;
        if (!hasNewGoal) return;

        // new goal will be set after a random delay
        isSettingGoal = true;
        float delay = UnityEngine.Random.Range(1, 5);
        IEnumerator coroutine = SetRandomGoalWithDelay(delay);
        StartCoroutine(coroutine);
    }

    private IEnumerator SetRandomGoalWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (State != StalkerState.Bored)
        {
            isSettingGoal = false;
            yield return null;
        }
        else
        {
            int xRange = 70;
            int zRange = 40;

            float xCoord = UnityEngine.Random.Range(campfire.transform.position.x - xRange / 2, campfire.transform.position.x + xRange / 2);
            float zCoord = UnityEngine.Random.Range(campfire.transform.position.z - zRange / 2, campfire.transform.position.z + zRange / 2);

            SetGoal(new Vector3(xCoord, transform.position.y, zCoord));
            isSettingGoal = false;
        }
    }

    public void SetGoal(Vector3 newGoal)
    {
        GoalPosition = new Vector3(newGoal.x, transform.position.y, newGoal.z);
        //Debug.LogFormat("New goal for {0}: {1}", Name, GoalPosition);
    }

    // --****========== artifact ==========***-- //

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
        State = StalkerState.Bored;
        //State &= ~StalkerState.DetectedArtifact;
    }


    // --****========== vision frustum checks ==========***-- //

    private void LookForArtifacts()
    {
        if (State == StalkerState.DetectedArtifact) return;
        Look(ArtifactLayerMask);
    }

    private void LookForObstacles()
    {
        Look(ObstacleLayerMask);
    }

    private void Look(int layermask)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, StalkerData.VisionRange, layermask);
        if (colliders.Length > 0)
        {
            // get nearest artifact and set as goal
            float minDistance = StalkerData.VisionRange;
            GameObject nearestArtifact = null;

            foreach (Collider collider in colliders)
            {
                Vector3 direction = collider.transform.position - transform.position;
                Vector3 xzDirection = new Vector2(direction.x, direction.z);
                float distance = xzDirection.magnitude;

                float dotProduct = Vector2.Dot(xzDirection.normalized, new Vector2(transform.forward.x, transform.forward.z));
                Artifact artifact = collider.gameObject?.GetComponent<Artifact>();
                if (dotProduct > 0.6 && artifact.IsVisible)
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestArtifact = collider.gameObject;
                        Debug.DrawRay(transform.position, direction, Color.red, 1f);
                    }
                    //Debug.LogFormat("{0} sees {1}, distance {2}, direction.normalized {3}, transform.forward {4}, dotProduct {5}, angle {6} ", Name, collider.gameObject.name, distance, xzDirection.normalized, transform.forward, dotProduct, Vector2.Angle(xzDirection, new Vector2(transform.forward.x, transform.forward.z)));
                }
            }

            if (nearestArtifact != null) 
            {
                TryCollectArtifact(nearestArtifact);
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}

    // --****========== static util functions ==========***-- //
    public static bool IsParallelxz(Vector3 a, Vector3 b)
    {
        float tolerance = 0.02f;
        return CloseEnough(a.x, b.x, tolerance) && CloseEnough(a.z, b.z, tolerance);
    }
    public static bool CloseEnough(float x, float y, float tolerance)
    {
        return Math.Abs(x - y) <= tolerance;
    }
}
