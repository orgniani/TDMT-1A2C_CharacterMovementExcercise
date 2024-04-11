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
    [SerializeField] private string isFallingParameter = "is_falling";
    [SerializeField] private string horSpeedParameter = "hor_speed";

    private void OnEnable()
    {
        if (jump)
        {
            jump.onJump += HandleJump;
            ledgeGrab.onClimb += HandleClimb;
        }

    }

    private void OnDisable()
    {
        if (jump)
        {
            jump.onJump -= HandleJump;
            ledgeGrab.onClimb -= HandleClimb;
        }
    }

    private void Update()
    {
        if (!rigidBody)
            return;

        var velocity = rigidBody.velocity;
        velocity.y = 0;
        var speed = velocity.magnitude;

        if (animator)
            animator.SetFloat(horSpeedParameter, speed);

        if (animator && body)
        {
            animator.SetBool(isFallingParameter, body.IsFalling);
        }
    }

    private void HandleJump()
    {
        if (!animator) return;

        animator.SetTrigger(jumpTriggerParameter);

        //if (velocity == Vector3.zero)
        //  animator.SetTrigger(jumpTriggerParameter);

        //else
        // animator.SetTrigger(runningJumpTriggerParameter);
    }

    private void HandleClimb()
    {
        if(!animator) return;
        animator.SetTrigger(climbTriggerParameter);
    }
}
