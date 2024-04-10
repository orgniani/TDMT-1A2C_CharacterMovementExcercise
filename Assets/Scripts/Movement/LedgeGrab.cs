using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    [SerializeField] private CharacterBody body;
    [SerializeField] private Rigidbody rigidBody;

    [SerializeField] private Collider capsuleCollider;
    [SerializeField] private Collider sphereCollider;

    [SerializeField] private CharacterBrain brain;
    [SerializeField] private RotationBasedOnVelocity rotatePlayer;

    [SerializeField] private LayerMask floorMask;

    private bool isHanging = false;
    private bool shouldClimb = true;

    [SerializeField] private float climbForce = 3;
    [SerializeField] private float waitToClimb = 0.5f;

    public event Action onClimb = delegate { };

    private void FixedUpdate()
    {
        if (!body.IsFalling) return;
        if (rigidBody.velocity.y < 0 && !isHanging)
        {
            if (!shouldClimb) return;

            RaycastHit downHit;

            //Vector3 lineDownStart = (transform.position + Vector3.up * 1.5f) + transform.forward;

            Vector3 lineDownStart = (transform.position + Vector3.up * 1f) + transform.forward;
            Vector3 lineDownEnd = (transform.position + Vector3.up * 0.7f) + transform.forward;

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

                    isHanging = true;
  
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
    }

    private void StopMovingWhenHanging()
    {
        brain.enabled = false;
        rotatePlayer.enabled = false;
        capsuleCollider.enabled = false;
        sphereCollider.enabled = false;
        body.SetMovement(new MovementRequest(Vector3.zero, 0f, 0f));
    }

    private void StartMovingWhenStopHanging()
    {
        brain.enabled = true;
        rotatePlayer.enabled = true;
        capsuleCollider.enabled = true;
        sphereCollider.enabled = true;
    }

    private IEnumerator ClimbSequence()
    {
        shouldClimb = false;

        onClimb.Invoke();

        yield return new WaitForSeconds(waitToClimb);

        rigidBody.useGravity = true;
        isHanging = false;

        body.RequestImpulse(new ImpulseRequest(Vector3.up, climbForce));
        body.RequestImpulse(new ImpulseRequest(transform.forward, climbForce/2));

        //body.SetMovement(new MovementRequest(transform.forward, climbForce * 2, climbForce * 2));

        rigidBody.useGravity = true;

        yield return new WaitForSeconds(0.3f);

        StartMovingWhenStopHanging();

        shouldClimb = true;
    }

}
