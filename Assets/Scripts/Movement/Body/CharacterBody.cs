using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

[RequireComponent(typeof(Rigidbody))]
public class CharacterBody : MonoBehaviour
{
    private Rigidbody rigidBody;
    private MovementRequest currentMovement = MovementRequest.InvalidRequest;
    private bool isBrakeRequested = false;
    private float brakeMultiplier = 1;

    private readonly List<ImpulseRequest> impulseRequests = new();

    private bool isOnAir = false;

    public bool IsFalling { private set; get; }

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

        if (brakeMultiplier == Model.LandBrakeMultiplier)
            StartCoroutine(Wait());

        else
            isBrakeRequested = false;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);

        isBrakeRequested = false;
    }

    private void ManageMovement()
    {
        var velocity = rigidBody.velocity;
        velocity.y = 0;

        RaycastHit hit;

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - Model.FloorSphereCheckOffset, transform.position.z);

        bool onFloorLineCheck = Physics.Raycast(transform.position + Model.FloorLineCheckOffset, -transform.up, out hit, Model.MaxFloorDistance, Model.FloorMask);
        bool onFloorSphereCheck = Physics.CheckSphere(spherePosition, 0.3f, Model.FloorMask, QueryTriggerInteraction.Ignore);

        IsFalling = !onFloorLineCheck && !onFloorSphereCheck;

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
        if (Model.FloorMask == (Model.FloorMask | (1 << collision.gameObject.layer)))
        {
            if (!isOnAir) return;

            RequestBrake(Model.LandBrakeMultiplier);
            isOnAir = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position + Model.FloorLineCheckOffset, -transform.up * Model.MaxFloorDistance);

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - Model.FloorSphereCheckOffset, transform.position.z);
        Gizmos.DrawWireSphere(spherePosition, 0.2f);
    }

}