using System;
using UnityEngine;

[Serializable]
public class JumpModel
{
    [field: SerializeField] public float Force { get; private set; } = 5.2f;

    [field: SerializeField] public float FloorAngle { get; private set; } = 45;

    [field: SerializeField] public float BrakeMultiplier { get; private set; } = 0.8f;

    [field: SerializeField] public float Acceleration { get; private set; } = 8f;

    [field: SerializeField] public float Cooldown { get; private set; } = 1f;

    [field: SerializeField] public float WaitToJump { get; private set; } = 0.3f;
}
