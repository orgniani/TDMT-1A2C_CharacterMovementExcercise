using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterBody : MonoBehaviour
{
    [SerializeField] private LayerMask floorMask;

    private Rigidbody rigidBody;

    private MovementRequest currentMovement = MovementRequest.InvalidRequest;
    private bool isBrakeRequested = false;
    private float brakeMultiplier = 1;

    private readonly List<ImpulseRequest> impulseRequests = new();

    private bool isOnAir = false;
    private bool shouldCheckIfOnLand = true;

    public bool IsFalling { private set; get; }

    public bool IsOnLand { private set; get; }

    public BodyModel Model { get; set; }

    private void Reset()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isBrakeRequested)
        {
            Break();
        }

        ManageMovement();
        ManageImpulseRequests();
    }

    public void SetMovement(MovementRequest movementRequest)
    {
        currentMovement = movementRequest;
    }

    public void RequestBrake(float brake)
    {
        brakeMultiplier = brake;
        isBrakeRequested = true;
    }

    public void RequestImpulse(ImpulseRequest request)
    {
        impulseRequests.Add(request);
    }

    private void Break()
    {
        rigidBody.AddForce(-rigidBody.velocity * brakeMultiplier, ForceMode.Impulse);
        isBrakeRequested = false;
    }

    private void ManageMovement()
    {
        var velocity = rigidBody.velocity;
        velocity.y = 0;

        RaycastHit hit;

        Vector3 lineOffset = new Vector3(0f, Model.FloorLineCheckOffset, 0f);
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - Model.FloorSphereCheckOffset, transform.position.z);

        bool onFloorLineCheck = Physics.Raycast(transform.position + lineOffset, -transform.up, out hit, Model.MaxFloorDistance, floorMask);
        bool onFloorSphereCheck = Physics.CheckSphere(spherePosition, Model.FloorSphereCheckRadius, floorMask, QueryTriggerInteraction.Ignore);

        IsFalling = !onFloorLineCheck && !onFloorSphereCheck;

        CheckIfOnLand(onFloorLineCheck, onFloorSphereCheck);

        if (!currentMovement.IsValid() || velocity.magnitude >= currentMovement.GoalSpeed)
            return;

        var accelerationVector = currentMovement.GetAccelerationVector();

        if (!IsFalling)
        {
            accelerationVector = Vector3.ProjectOnPlane(accelerationVector, hit.normal);
            Debug.DrawRay(transform.position, accelerationVector, Color.cyan);
        }

        else isOnAir = true;

        Debug.DrawRay(transform.position, accelerationVector, Color.red);
        rigidBody.AddForce(accelerationVector, ForceMode.Force);
    }

    private void CheckIfOnLand(bool onFloorLineCheck, bool onFloorSphereCheck)
    {
        if (onFloorLineCheck)
        {
            if (!shouldCheckIfOnLand) return;
            IsOnLand = onFloorSphereCheck;
        }

        else
        {
            if (onFloorSphereCheck) IsOnLand = true;

            else
            {
                shouldCheckIfOnLand = true;
                IsOnLand = false;
            }
        }

        if (IsOnLand == true) shouldCheckIfOnLand = false;
    }

    private void ManageImpulseRequests()
    {
        foreach (var request in impulseRequests)
        {
            rigidBody.AddForce(request.GetForceVector(), ForceMode.Impulse);
        }

        impulseRequests.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (floorMask == (floorMask | (1 << collision.gameObject.layer)))
        {
            if (!isOnAir) return;
            RequestBrake(Model.LandBrakeMultiplier);
            isOnAir = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (Model == null) return;

        Gizmos.color = Color.green;

        Vector3 floorLineCheck = new Vector3(0f, Model.FloorLineCheckOffset, 0f);
        Gizmos.DrawRay(transform.position + floorLineCheck, -transform.up * Model.MaxFloorDistance);

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - Model.FloorSphereCheckOffset, transform.position.z);
        Gizmos.DrawWireSphere(spherePosition, Model.FloorSphereCheckRadius);
    }

}