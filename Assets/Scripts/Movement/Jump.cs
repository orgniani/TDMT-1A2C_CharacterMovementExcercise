using System;
using System.Collections;
using UnityEngine;
public class Jump : MonoBehaviour
{
    [SerializeField] private CharacterBody body;
    [SerializeField] private Rigidbody rigidBody;
    //[SerializeField] private JumpModel model;

    [SerializeField] private float jumpForce = 10;
    [SerializeField] private int floorAngle = 30;

    [SerializeField] private float waitToJump = 0.5f;

    private bool shouldJump = true;
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

        body.RequestBrake();

        onJump.Invoke();

        yield return new WaitForSeconds(waitToJump);

        body.RequestImpulse(new ImpulseRequest(Vector3.up, jumpForce));

        yield return new WaitForSeconds(jumpCooldown);

        shouldJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.contacts[0];
        var contactAngle = Vector3.Angle(contact.normal, Vector3.up);

        if (contactAngle <= floorAngle)
        {
            shouldJumpOnRamp = true;

            if (body.IsFalling)
            {
                body.RequestBrake();
            }
        }

        else
        {
            shouldJumpOnRamp = false;
        }

        if (enableLog)
            Debug.Log($"{name}: Collided with normal angle: {contactAngle}");
    }

}
