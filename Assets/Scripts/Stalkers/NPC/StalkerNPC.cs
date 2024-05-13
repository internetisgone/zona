using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StalkerNPC : CStalker
{
    public static int GroundLayerMask = 1 << 6;
    public static int ArtifactLayerMask = 1 << 7;
    private static int ObstacleLayerMask = 1 << 8;

    public static BoredState BoredState = new BoredState();
    public static ArtifactState ArtifactState = new ArtifactState();
    public static ChillingState ChillingState = new ChillingState();
    public static PanicState PanicState = new PanicState();

    public NPCData StalkerData;
    private StalkerState currentState;

    private Vector3 GoalPosition;
    private Vector3 movement;
    private Vector3 forward;

    private bool isSettingGoal = false;

    private static GameObject campfire;
    private static int RandomGoalRangeX = 70;
    private static int RandomGoalRangeZ = 40;

    private DetectorNPC Detector;
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
        Detector = GetComponent<DetectorNPC>();

        isOnSlope = false;
        slopeNormal = Vector3.up;

        SetRandomGoal();
        ChangeState(BoredState);
    }

    void FixedUpdate()
    {
        movement = GoalPosition - transform.position;
        forward = new Vector3(movement.x, 0, movement.z);

        CheckGround();

        if (isOnSlope)
        {
            Vector3 slidingForce = Vector3.ProjectOnPlane(rb.mass * Physics.gravity, slopeNormal);
            rb.AddForce(-slidingForce);
        }

        currentState?.OnUpdate(this);
    }

    // --****========== movement ==========***-- //

    public void MoveToGoal()
    {
        // check if facing the goal
        bool shouldTurn = !IsParallelxz(forward.normalized, transform.forward);

        if (shouldTurn)
        {
            // turn to goal
            KeepStill();
            TurnTowards(forward);
            animator.SetBool("IsTurning", true); // todo only needs to be set once 
            Debug.DrawRay(transform.position, transform.forward * 2, Color.white, 1f);
        }
        else
        {
            // move to goal
            MoveStalker(movement.normalized * StalkerData.Speed);
            animator.SetBool("IsTurning", false);
            animator.SetFloat("Speed", new Vector2(rb.velocity.x, rb.velocity.z).magnitude);
            Debug.DrawRay(transform.position, transform.forward * 2, Color.blue, 1f);
        }
    }

    public bool IsAtGoalPosition()
    {
        return forward.magnitude < 2;
    }

    public void KeepStill()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        animator.SetBool("IsTurning", false);
        animator.SetFloat("Speed", 0); 
    }

    private void MoveStalker(Vector3 targetMovement)
    {
        if (isOnSlope)
        {
            targetMovement = Vector3.ProjectOnPlane(targetMovement, slopeNormal);
        }
        else
        {
            targetMovement = Vector3.Lerp(rb.velocity, targetMovement, 0.2f);
        }

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

    public void StartSettingRandomGoal()
    {
        InvokeRepeating("TrySetRandomGoal", 10, 7);
    }

    public void StopSettingRandomGoal()
    {
        CancelInvoke("TrySetRandomGoal");
    }
    public void TrySetRandomGoal()
    {
        if (currentState != BoredState || isSettingGoal) return;

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

        if (currentState == BoredState)
        { 
            SetRandomGoal();
        }
        isSettingGoal = false;
    }

    private void SetRandomGoal()
    {
        float xCoord = UnityEngine.Random.Range(campfire.transform.position.x - RandomGoalRangeX / 2, campfire.transform.position.x + RandomGoalRangeX / 2);
        float zCoord = UnityEngine.Random.Range(campfire.transform.position.z - RandomGoalRangeZ / 2, campfire.transform.position.z + RandomGoalRangeZ / 2);

        SetGoal(new Vector3(xCoord, transform.position.y, zCoord));
    }

    private void SetGoal(Vector3 newGoal)
    {
        GoalPosition = new Vector3(newGoal.x, transform.position.y, newGoal.z);
        //Debug.LogFormat("New goal for {0}: {1}", Name, GoalPosition);
    }

    // --****========== artifact ==========***-- //

    public void SetArtifactAsGoal(GameObject artifact)
    {
        goalArtifact = artifact;
        SetGoal(goalArtifact.transform.position);
        ChangeState(ArtifactState);
    }

    public void CollectGoalArtifact()
    {
        base.CollectArtifact(goalArtifact);
        goalArtifact = null;
    }

    public void EquipDetector()
    {
        Detector.enabled = true;
    }

    public void UnequipDetector()
    {
        Detector.enabled = false;
    }

    // --****========== vision frustum checks ==========***-- //

    public void StartLookForArtifacts()
    {
        InvokeRepeating("LookForArtifacts", 1, StalkerData.LookInterval);
    }

    public void StopLookingForArtifacts()
    {
        CancelInvoke("LookForArtifacts");
    }

    private void LookForArtifacts()
    {
        Look(ArtifactLayerMask);
    }

    private void Look(int layermask)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, StalkerData.VisionRange, layermask);
        if (colliders.Length > 0)
        {
            // get nearest visible artifact and set as goal
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
                SetArtifactAsGoal(nearestArtifact);
            }
        }
    }

    // --****========== state transition ==========***-- //
    public void ChangeState(StalkerState newState)
    {
        if (currentState == PanicState) return;

        currentState?.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);
    }

    public override void Panic()
    {
        ChangeState(PanicState);
    }

    public void PlayPanicAnim()
    {
        animator.SetTrigger("Panic");
    }

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
