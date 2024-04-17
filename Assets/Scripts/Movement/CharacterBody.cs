using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterBody : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float maxFloorDistance = .01f;
    [SerializeField] private float landBrakeMultiplier = 1;
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Vector3 floorCheckOffset = new Vector3(0, 0.001f, 0);

    private Rigidbody rigidBody;
    private MovementRequest currentMovement = MovementRequest.InvalidRequest;
    private bool isBrakeRequested = false;
    private float brakeMultiplier = 1;

    private readonly List<ImpulseRequest> impulseRequests = new();

    private bool isOnAir = false;
    public bool IsFalling { private set; get; }

    private void Reset()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (floorMask != LayerMask.GetMask("Floor"))
        {
            Debug.LogError($"{name}: {nameof(floorMask)} is not set on floor layer!");
        }
    }

    private void FixedUpdate()
    {
        if (isBrakeRequested)
        {
            Break();
            return;
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

        if (brakeMultiplier == landBrakeMultiplier)
            StartCoroutine(Wait());

        else
            isBrakeRequested = false;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);

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

        else isOnAir = true;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (floorMask == (floorMask | (1 << collision.gameObject.layer)))
        {
            if (!isOnAir) return;

            RequestBrake(landBrakeMultiplier);
            isOnAir = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + floorCheckOffset, -transform.up * maxFloorDistance);
    }

}