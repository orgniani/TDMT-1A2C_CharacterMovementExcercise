using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Models/Jump", fileName = "JM_")]
public class JumpModelContainer : ScriptableObject
{
    [field: SerializeField] public JumpModel Model { get; private set; }
}
