using System;
using System.Collections;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterBody body;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Collider bodyCollider;
    [SerializeField] private Collider feetCollider;
    [SerializeField] private RotationBasedOnVelocity rotatePlayer;

    [Header("Parameters")]

    [SerializeField] private float lineStartOffset = 1f;
    [SerializeField] private float lineEndOffset = 0.7f;

    [SerializeField] private float climbForce = 3;
    [SerializeField] private float waitToClimb = 0.5f;
    [SerializeField] private float waitToReEnableComponents = 0.3f;
    [SerializeField] private LayerMask floorMask;

    private bool shouldClimb = true;

    public event Action onClimb = delegate { };

    public bool IsHanging { private set; get; }

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

        if (!rigidBody)
        {
            Debug.LogError($"{name}: {nameof(rigidBody)} is null!" +
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

        if (floorMask != LayerMask.GetMask("Floor"))
        {
            Debug.LogError($"{name}: {nameof(floorMask)} is not set on floor layer!");
        }
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

        Vector3 lineDownStart = (transform.position + Vector3.up * lineStartOffset) + transform.forward;
        Vector3 lineDownEnd = (transform.position + Vector3.up * lineEndOffset) + transform.forward;

        Physics.Linecast(lineDownStart, lineDownEnd, out downHit, floorMask);
        Debug.DrawLine(lineDownStart, lineDownEnd);

        if (downHit.collider != null)
        {
            RaycastHit fwdHit;
            Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
            Vector3 lineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;

            Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit, floorMask);
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

        yield return new WaitForSeconds(waitToClimb);

        rigidBody.useGravity = true;

        body.RequestImpulse(new ImpulseRequest(Vector3.up, climbForce));
        body.RequestImpulse(new ImpulseRequest(transform.forward, climbForce/2));

        yield return new WaitForSeconds(waitToReEnableComponents);

        StartMovingWhenStopHanging();

        yield return new WaitForSeconds(waitToReEnableComponents);

        IsHanging = false;

        shouldClimb = true;
    }

}
