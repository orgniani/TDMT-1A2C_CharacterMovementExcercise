using System;
using UnityEngine;

[Serializable]
public class LedgeGrabModel
{
    [field: SerializeField] public float LineStartOffset { get; private set; } = 1f;

    [field: SerializeField] public float LineEndOffset { get; private set; } = 0.7f;

    [field: SerializeField] public float ClimbForce { get; private set; } = 4f;

    [field: SerializeField] public float WaitToClimb { get; private set; } = 0.5f;

    [field: SerializeField] public float WaitToReEnableComponents { get; private set; } = 0.3f;

    [field: SerializeField] public LayerMask FloorMask { get; private set; }
}
