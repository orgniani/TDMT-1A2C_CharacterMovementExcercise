using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterBody : MonoBehaviour
{
    [SerializeField] private float maxFloorDistance = .01f;
    [SerializeField] private float brakeMultiplier = 1;

    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Vector3 floorCheckOffset = new Vector3(0, 0.001f, 0);

    private Rigidbody rigidBody;
    private MovementRequest currentMovement = MovementRequest.InvalidRequest;
    private bool isBrakeRequested = false;

    private readonly List<ImpulseRequest> impulseRequests = new();

    public bool IsFalling { private set; get; }

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
            Break();

        ManageMovement();
        ManageImpulseRequests();
    }

    public void SetMovement(MovementRequest movementRequest)
    {
        currentMovement = movementRequest;
    }

    public void RequestBrake()
    {
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

        IsFalling = !Physics.Raycast(transform.position + floorCheckOffset, -transform.up, out hit, maxFloorDistance, floorMask);

        if (!currentMovement.IsValid() || velocity.magnitude >= currentMovement.GoalSpeed)
            return;

        var accelerationVector = currentMovement.GetAccelerationVector();

        if (!IsFalling)
        {
            accelerationVector = Vector3.ProjectOnPlane(accelerationVector, hit.normal);
            Debug.DrawRay(transform.position, accelerationVector, Color.cyan);
        }

        Debug.DrawRay(transform.position, accelerationVector, Color.red);
        rigidBody.AddForce(accelerationVector, ForceMode.Force);
    }

    private void ManageImpulseRequests()
    {
        foreach (var request in impulseRequests)
        {
            rigidBody.AddForce(request.GetForceVector(), ForceMode.Impulse);
        }

        impulseRequests.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + floorCheckOffset, -transform.up * maxFloorDistance);
    }
}
