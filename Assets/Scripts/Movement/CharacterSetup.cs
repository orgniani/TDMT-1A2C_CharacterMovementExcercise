using UnityEngine;

public class CharacterSetup : MonoBehaviour
{
    [Header("Character Body")]
    [SerializeField] private BodyModelContainer bodyModelContainer;
    [SerializeField] private CharacterBody characterBody;

    [Header("Character Brain")]
    [SerializeField] private BrainModelContainer brainModelContainer;
    [SerializeField] private CharacterBrain characterBrain;

    [Header("Jump")]
    [SerializeField] private JumpModelContainer jumpModelContainer;
    [SerializeField] private Jump jump;

    [Header("Rotation")]
    [SerializeField] private RotationModelContainer rotationModelContainer;
    [SerializeField] private RotationBasedOnVelocity rotation;

    [Header("LedgeGrab")]
    [SerializeField] private LedgeGrabModelContainer ledgeGrabModelContainer;
    [SerializeField] private LedgeGrab ledgeGrab;

    private void OnEnable()
    {
        if (jump && jumpModelContainer)
        {
            jump.Model = jumpModelContainer.Model;
            jump.enabled = true;
        }

        else
        {
            Debug.LogError($"{name}: {nameof(jump)} or {nameof(jumpModelContainer)} is null!" +
               $"\nDisabling component to avoid errors.");
        }

        if (characterBody && bodyModelContainer)
        {
            characterBody.Model = bodyModelContainer.Model;
            characterBody.enabled = true;
        }

        else
        {
            Debug.LogError($"{name}: {nameof(characterBody)} or {nameof(bodyModelContainer)} is null!" +
               $"\nDisabling component to avoid errors.");
        }

        if (characterBrain && brainModelContainer)
        {
            characterBrain.Model = brainModelContainer.Model;
            characterBrain.enabled = true;
            characterBrain.Acceleration = brainModelContainer.Model.Acceleration;
        }

        else
        {
            Debug.LogError($"{name}: {nameof(characterBrain)} or {nameof(brainModelContainer)} is null!" +
               $"\nDisabling component to avoid errors.");
        }

        if (rotation && rotationModelContainer)
        {
            rotation.Model = rotationModelContainer.Model;
            rotation.enabled = true;
        }

        else
        {
            Debug.LogError($"{name}: {nameof(rotation)} or {nameof(rotationModelContainer)} is null!" +
               $"\nDisabling component to avoid errors.");
        }

        if (ledgeGrab && ledgeGrabModelContainer)
        {
            ledgeGrab.Model = ledgeGrabModelContainer.Model;
            ledgeGrab.enabled = true;
        }

        else
        {
            Debug.LogError($"{name}: {nameof(ledgeGrab)} or {nameof(ledgeGrabModelContainer)} is null!" +
               $"\nDisabling component to avoid errors.");
        }
    }
}
