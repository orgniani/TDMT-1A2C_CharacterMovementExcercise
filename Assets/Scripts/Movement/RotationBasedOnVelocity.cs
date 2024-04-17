using UnityEngine;

public class RotationBasedOnVelocity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rigidBody;

    [Header("Parameters")]
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float minimumSpeedForRotation = 0.001f;

    private void Awake()
    {
        if (!rigidBody)
        {
            Debug.LogError($"{name}: {nameof(rigidBody)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        RotateCharacter();
    }

    private void RotateCharacter()
    {
        Vector3 velocity = rigidBody.velocity;
        velocity.y = 0;

        if (velocity.magnitude < minimumSpeedForRotation)
            return;

        float rotationAngle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
        transform.Rotate(Vector3.up, rotationAngle * rotationSpeed * Time.deltaTime);
    }
}
