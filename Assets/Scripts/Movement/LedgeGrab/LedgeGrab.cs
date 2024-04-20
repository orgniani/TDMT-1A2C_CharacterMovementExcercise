using System;
using System.Collections;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterBody body;
    [SerializeField] private Collider bodyCollider;
    [SerializeField] private Collider feetCollider;
    [SerializeField] private RotationBasedOnVelocity rotatePlayer;

    private Rigidbody rigidBody;

    private bool shouldClimb = true;

    public event Action onClimb = delegate { };

    public bool IsHanging { private set; get; }

    public LedgeGrabModel Model { get; set; }

    private void Reset()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        IsHanging = false;

        if (!body)
        {
            Debug.LogError($"{name}: {nameof(body)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!bodyCollider)
        {
            Debug.LogError($"{name}: {nameof(bodyCollider)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!feetCollider)
        {
            Debug.LogError($"{name}: {nameof(feetCollider)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!rotatePlayer)
        {
            Debug.LogError($"{name}: {nameof(rotatePlayer)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!body.IsFalling) return;

        if (rigidBody.velocity.y < 0 && !IsHanging)
        {
            if (!shouldClimb) return;

            DetectEdge();
        }
    }

    private void DetectEdge()
    {
        RaycastHit downHit;

        Vector3 lineDownStart = (transform.position + Vector3.up * Model.LineStartOffset) + transform.forward;
        Vector3 lineDownEnd = (transform.position + Vector3.up * Model.LineEndOffset) + transform.forward;

        Physics.Linecast(lineDownStart, lineDownEnd, out downHit, Model.FloorMask);
        Debug.DrawLine(lineDownStart, lineDownEnd);

        if (downHit.collider != null)
        {
            RaycastHit fwdHit;
            Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
            Vector3 lineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;

            Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit, Model.FloorMask);
            Debug.DrawLine(lineFwdStart, lineFwdEnd);

            if (fwdHit.collider != null)
            {
                rigidBody.useGravity = false;
                rigidBody.velocity = Vector3.zero;

                IsHanging = true;

                Vector3 hangingPosition = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                Vector3 offset = transform.forward * -0.2f + transform.up * -0.8f;
                hangingPosition += offset;

                transform.position = hangingPosition;
                transform.forward = -fwdHit.normal;

                StopMovingWhenHanging();

                StartCoroutine(ClimbSequence());
            }

        }
    }

    private void StopMovingWhenHanging()
    {
        rotatePlayer.enabled = false;
        bodyCollider.enabled = false;
        feetCollider.enabled = false;
    }

    private void StartMovingWhenStopHanging()
    {
        rotatePlayer.enabled = true;
        bodyCollider.enabled = true;
        feetCollider.enabled = true;
    }

    private IEnumerator ClimbSequence()
    {
        body.SetMovement(new MovementRequest(Vector3.zero, 0f, 0f));

        shouldClimb = false;
        onClimb.Invoke();

        yield return new WaitForSeconds(Model.WaitToClimb);

        rigidBody.useGravity = true;

        body.RequestImpulse(new ImpulseRequest(Vector3.up, Model.ClimbForce));
        body.RequestImpulse(new ImpulseRequest(transform.forward, Model.ClimbForce / 2));

        yield return new WaitForSeconds(Model.WaitToReEnableComponents);

        StartMovingWhenStopHanging();

        yield return new WaitForSeconds(Model.WaitToReEnableComponents);

        IsHanging = false;

        shouldClimb = true;
    }

}
