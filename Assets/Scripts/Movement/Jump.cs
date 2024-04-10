using System;
using System.Collections;
using UnityEngine;
public class Jump : MonoBehaviour
{
    [SerializeField] private CharacterBody body;
    [SerializeField] private Rigidbody rigidBody;
    //[SerializeField] private JumpModel model;

    [SerializeField] private float waitToJump = 0.5f;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private int floorAngle = 30;

    [SerializeField] private float jumpBrakeMultiplier = 1f;

    [SerializeField] private float waitToFall = 0.3f;
    [SerializeField] private float fallSpeed = 1f;
    [SerializeField] private float fallAcceleration = 1f;

    [SerializeField] private LayerMask floorMask;

    private bool shouldJump = true;
    private bool isJumping = true;
    [SerializeField] private float jumpCooldown = 1f;

    private bool shouldJumpOnRamp = true;

    [SerializeField] private bool enableLog = true;

    public event Action onJump = delegate { };

    private void Reset()
    {
        body = GetComponent<CharacterBody>();
    }

    public bool TryJump()
    {
        if (!shouldJump) return false;

        if (!shouldJumpOnRamp) return false;

        StartCoroutine(JumpSequence());

        return true;
    }

    private IEnumerator JumpSequence()
    {
        shouldJump = false;

        body.RequestBrake(jumpBrakeMultiplier);

        onJump.Invoke();

        isJumping = true;

        yield return new WaitForSeconds(waitToJump);

        body.RequestImpulse(new ImpulseRequest(Vector3.up, jumpForce));

        //yield return new WaitForSeconds(waitToFall);

        //body.SetMovement(new MovementRequest(Vector3.down, fallSpeed, fallAcceleration));

        yield return new WaitForSeconds(jumpCooldown);

        shouldJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.contacts[0];
        var contactAngle = Vector3.Angle(contact.normal, Vector3.up);

        if (contactAngle >= 90)
            contactAngle = 0;

        if (contactAngle <= floorAngle)
        {
            shouldJumpOnRamp = true;
        }

        else
        {
            shouldJumpOnRamp = false;
        }

        if (enableLog)
            Debug.Log($"{name}: Collided with normal angle: {contactAngle}");
    }

}
