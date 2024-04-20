using System;
using System.Collections;
using UnityEngine;
public class Jump : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterBody body;
    [SerializeField] private CharacterBrain brain;

    [Header("Logs")]
    [SerializeField] private bool enableLog = true;

    private bool shouldJump = true;
    private bool shouldJumpOnRamp = true;

    public event Action onJump = delegate { };

    public JumpModel Model { get; set; }

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
    }

    public bool TryJump()
    {
        if (!shouldJump) return false;

        if (!shouldJumpOnRamp) return false;

        if (body.IsFalling) return false;

        StartCoroutine(JumpSequence());

        return true;
    }

    private IEnumerator JumpSequence()
    {
        shouldJump = false;

        brain.Acceleration = Model.Acceleration;

        body.RequestBrake(Model.BrakeMultiplier);

        onJump.Invoke();

        yield return new WaitForSeconds(Model.WaitToJump);

        body.RequestImpulse(new ImpulseRequest(Vector3.up, Model.Force));

        yield return new WaitForSeconds(Model.Cooldown);

        shouldJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        brain.Acceleration = 15f; //CHANGE THIS TO VARIABLE!!!

        var contact = collision.contacts[0];
        var contactAngle = Vector3.Angle(contact.normal, Vector3.up);

        if (contactAngle >= 90)
            contactAngle = 0;

        if (contactAngle <= Model.FloorAngle)
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
