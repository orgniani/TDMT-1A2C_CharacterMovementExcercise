using System.Collections;
using System.Collections.Generic;
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

        if (characterBody && bodyModelContainer)
        {
            characterBody.Model = bodyModelContainer.Model;
            characterBody.enabled = true;
        }

        if (characterBrain && brainModelContainer)
        {
            characterBrain.Model = brainModelContainer.Model;
            characterBrain.enabled = true;
        }

        if (rotation && rotationModelContainer)
        {
            rotation.Model = rotationModelContainer.Model;
            rotation.enabled = true;
        }

        if (ledgeGrab && ledgeGrabModelContainer)
        {
            ledgeGrab.Model = ledgeGrabModelContainer.Model;
            ledgeGrab.enabled = true;
        }
    }
}
