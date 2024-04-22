using UnityEngine;

[CreateAssetMenu(menuName = "Models/CharacterBody", fileName = "CBM_")]
public class BodyModelContainer : ScriptableObject
{
    [field: SerializeField] public BodyModel Model { get; private set; }
}
