using System;
using System.Collections;
using UnityEngine;
public class Jump : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterBody body;
    [SerializeField] private Rigidbody rigidBody;
    //[SerializeField] private JumpModel model;

    [Header("Parameters")]
    [SerializeField] private float waitToJump = 0.5f;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private int floorAngle = 30;
    [SerializeField] private float jumpBrakeMultiplier = 1f;
    [SerializeField] private float jumpCooldown = 1f;

    [Header("Logs")]
    [SerializeField] private bool enableLog = true;

    private bool shouldJump = true;
    private bool shouldJumpOnRamp = true;

    public event Action onJump = delegate { };

    private void Reset()
    {
        body = GetComponent<CharacterBody>();
    }

    private void Awake()
    {
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
    }

    public bool TryJump()
    {
        if (!shouldJump) return false;

        if (!shouldJumpOnRamp) return false;

        if(body.IsFalling) return false;

        StartCoroutine(JumpSequence());

        return true;
    }

    private IEnumerator JumpSequence()
    {
        shouldJump = false;

        body.RequestBrake(jumpBrakeMultiplier);

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
