using System;
using UnityEngine;

[Serializable]
public class BodyModel
{
    [field: SerializeField] public float MaxFloorDistance { get; private set; } = 0.5f;

    [field: SerializeField] public float LandBrakeMultiplier { get; private set; } = 1f;

    [field: SerializeField] public LayerMask FloorMask { get; private set; }

    [field: SerializeField] public Vector3 FloorLineCheckOffset { get; private set; } = new Vector3(0, 0.1f, 0);

    [field: SerializeField] public float FloorSphereCheckOffset { get; private set; } = 0.1f;
}
