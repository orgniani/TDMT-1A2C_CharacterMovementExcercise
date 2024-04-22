using UnityEngine;

public class AnimatorView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Jump jump;
    [SerializeField] private LedgeGrab ledgeGrab;
    [SerializeField] private CharacterBody body;

    [Header("Animator Parameters")]
    [SerializeField] private string jumpTriggerParameter = "jump";
    [SerializeField] private string climbTriggerParameter = "climb";
    [SerializeField] private string isOnLandParameter = "on_land";
    [SerializeField] private string horSpeedParameter = "hor_speed";

    private void Awake()
    {
        if (!animator)
        {
            Debug.LogError($"{name}: {nameof(animator)} is null!" +
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

        if (!jump)
        {
            Debug.LogError($"{name}: {nameof(jump)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!ledgeGrab)
        {
            Debug.LogError($"{name}: {nameof(ledgeGrab)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!body)
        {
            Debug.LogError($"{name}: {nameof(body)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        jump.onJump += HandleJump;
        ledgeGrab.onClimb += HandleClimb;
    }

    private void OnDisable()
    {
        jump.onJump -= HandleJump;
        ledgeGrab.onClimb -= HandleClimb;
    }

    private void Update()
    {
        var velocity = rigidBody.velocity;
        velocity.y = 0;
        var speed = velocity.magnitude;

        animator.SetFloat(horSpeedParameter, speed);

        animator.SetBool(isOnLandParameter, body.IsOnLand);
    }

    private void HandleJump()
    {
        animator.SetTrigger(jumpTriggerParameter);
    }

    private void HandleClimb()
    {
        animator.SetTrigger(climbTriggerParameter);
    }
}
