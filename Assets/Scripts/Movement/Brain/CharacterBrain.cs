using UnityEngine;

public class CharacterBrain : MonoBehaviour
{
    [SerializeField] private CharacterBody body;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Jump jump;
    [SerializeField] private LedgeGrab grab;
    [SerializeField] private FollowPlayer cameraController;
    [SerializeField] private Transform cameraTransform;

    private Vector3 desiredDirection;
    private Vector2 input;

    private float acceleration;

    public BrainModel Model { get; set; }

    public float Acceleration
    {
        set { acceleration = value; }
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

        if (!inputReader)
        {
            Debug.LogError($"{name}: {nameof(inputReader)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!jump)
        {
            Debug.LogError($"{name}: {nameof(jump)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!grab)
        {
            Debug.LogError($"{name}: {nameof(grab)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!cameraController)
        {
            Debug.LogError($"{name}: {nameof(cameraController)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!cameraTransform)
        {
            Debug.LogError($"{name}: {nameof(cameraTransform)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
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

    private void Update()
    {
        if (grab.IsHanging) return;

        if (desiredDirection.magnitude > Mathf.Epsilon && input.magnitude < Mathf.Epsilon)
        {
            body.RequestBrake(Model.MovementBreakMultiplier);
        }

        Vector3 movementInput = input;

        desiredDirection = TransformDirectionRelativeToCamera(movementInput);

        body.SetMovement(new MovementRequest(desiredDirection, Model.Speed, acceleration));
    }

    private void HandleMovementInput(Vector2 input)
    {
        this.input = input;
    }

    private Vector3 TransformDirectionRelativeToCamera(Vector2 input)
    {
        Vector3 direction = new Vector3(input.x, 0, input.y);

        if (cameraTransform)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;

            direction = Quaternion.LookRotation(cameraForward) * direction;
        }

        return direction.normalized; 
    }

    private void HandleCameraInput(Vector2 input)
    {
        cameraController.SetInputRotation(input);
    }

    private void HandleJumpInput()
    {
        if (grab.IsHanging) return;
        jump.TryJump(Model.Acceleration);
    }
}
