using System;
using UnityEngine;

[Serializable]
public class BrainModel
{
    [field: SerializeField] public float Speed { get; private set; } = 8f;

    [field: SerializeField] public float Acceleration { get; set; } = 15f;

    [field: SerializeField] public float MovementBreakMultiplier { get; private set; } = 0.2f;
}
