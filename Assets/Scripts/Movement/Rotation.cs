using UnityEngine;
using UnityEngine.InputSystem;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1;

    private Vector2 rotationInput;

    private void Update()
    {
        rotationInput.x = Keyboard.current.aKey.isPressed ? -1f : Keyboard.current.dKey.isPressed ? 1f : 0f;

        RotateCharacter();
    }

    private void RotateCharacter()
    {
        float rotationAmount = rotationInput.x * rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, rotationAmount);
    }
}
