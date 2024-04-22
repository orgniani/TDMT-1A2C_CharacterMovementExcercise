using UnityEngine;

[CreateAssetMenu(menuName = "Models/Rotation", fileName = "RM_")]
public class RotationModelContainer : ScriptableObject
{
    [field: SerializeField] public RotationModel Model { get; private set; }
}
