using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private CharacterBody body;
    [SerializeField] private float jumpForce = 10;

    [SerializeField] private int maxJumpQty = 1;
    private int currentJumpQty = 0;

    [SerializeField] private float floorAngle = 30;
    [SerializeField] private bool enableLog = true;

    public event Action onJump = delegate { };
    public event Action onLand = delegate { };

    private void Reset()
    {
        body = GetComponent<CharacterBody>();
    }

    public bool TryJump()
    {
        if (currentJumpQty >= maxJumpQty)
        {
            return false;
        }


        currentJumpQty++;
        onJump.Invoke();

        body.RequestImpulse(new ImpulseRequest(Vector3.up, jumpForce));

        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.contacts[0];
        var contactAngle = Vector3.Angle(contact.normal, Vector3.up);

        if (contactAngle <= floorAngle)
        {
            currentJumpQty = 0;
            onLand.Invoke();
        }

        if (enableLog)
            Debug.Log($"{name}: Collided with normal angle: {contactAngle}");
    }
}
