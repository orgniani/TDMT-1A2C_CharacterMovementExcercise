using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Models/LedgeGrab", fileName = "LGM_")]
public class LedgeGrabModelContainer : ScriptableObject
{
    [field: SerializeField] public LedgeGrabModel Model { get; private set; }
}
