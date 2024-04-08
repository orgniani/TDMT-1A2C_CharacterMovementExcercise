using UnityEngine;

public class RotationBasedOnVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;

    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float minimumSpeedForRotation = 0.001f;

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
