using UnityEngine;

[CreateAssetMenu(menuName = "Models/Jump Model")]
public class JumpModel : ScriptableObject
{
    [SerializeField] public float JumpForce { get; private set; } = 10;
    [SerializeField] public int MaxQty { get; private set; } = 1;
    [SerializeField] public int FloorAngle { get; private set; } = 30;
}
