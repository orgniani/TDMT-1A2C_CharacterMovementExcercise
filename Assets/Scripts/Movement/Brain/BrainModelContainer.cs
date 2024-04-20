using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Models/CharacterBrain", fileName = "BRM_")]

public class BrainModelContainer : ScriptableObject
{
    [field: SerializeField] public BrainModel Model { get; private set; }
}
