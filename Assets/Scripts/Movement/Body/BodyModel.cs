using System;
using UnityEngine;

[Serializable]
public class BodyModel
{
    [field: SerializeField] public float MaxFloorDistance { get; private set; } = 0.5f;

    [field: SerializeField] public float LandBrakeMultiplier { get; private set; } = 1f;

    [field: SerializeField] public float FloorLineCheckOffset { get; private set; } = 0.1f;

    [field: SerializeField] public float FloorSphereCheckOffset { get; private set; } = 0.1f;

    [field: SerializeField] public float FloorSphereCheckRadius { get; private set; } = 0.3f;
}
