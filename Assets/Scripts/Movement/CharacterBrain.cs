using UnityEngine;

public class CharacterBrain : MonoBehaviour
{
    [SerializeField] private CharacterBody body;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Jump jump;

    [SerializeField] private float speed = 10;
    [SerializeField] private float acceleration = 4;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private bool enableLog = true;

    private Vector3 desiredDirection;

    private void Awake()
    {

        if (!body)
        {
            Debug.LogError($"{name}: {nameof(body)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!inputReader)
        {
            Debug.LogError($"{name}: {nameof(inputReader)} is null!");
            return;
        }
    }

    private void OnEnable()
    {
        if (!inputReader) return; 
        inputReader.onMovementInput += HandleMovementInput;
        inputReader.onJumpInput += HandleJumpInput;
        inputReader.OnCameraInput += HandleCameraInput;
    }

    private void OnDisable()
    {
        if (!inputReader) return;
        inputReader.onMovementInput -= HandleMovementInput;
        inputReader.onJumpInput -= HandleJumpInput;
        inputReader.OnCameraInput -= HandleCameraInput;
    }

    public Vector3 GetDesiredDirection() => desiredDirection;

    private void HandleMovementInput(Vector2 input)
    {
        if (desiredDirection.magnitude > Mathf.Epsilon
            && input.magnitude < Mathf.Epsilon)
        {
            if (enableLog)
            {
                Debug.Log($"{nameof(desiredDirection)} magnitude: {desiredDirection.magnitude}\t{nameof(input)} magnitude: {input.magnitude}");
            }

            body.RequestBrake();
        }

        desiredDirection = new Vector3(input.x, 0, input.y);

        if (cameraTransform)
        {
            desiredDirection = cameraTransform.TransformDirection(desiredDirection);
            desiredDirection.y = 0;
        }

        body.SetMovement(new MovementRequest(desiredDirection, speed, acceleration));
    }

    private void HandleCameraInput(Vector2 input)
    {
        //cameraController.SetInputRotation(input);
    }

    private void HandleJumpInput()
    {
        jump.TryJump();
    }
}
