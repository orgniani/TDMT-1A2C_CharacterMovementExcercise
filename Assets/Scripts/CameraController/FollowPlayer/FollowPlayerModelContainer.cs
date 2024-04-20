using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Models/FollowPlayer", fileName = "FPM_")]
public class FollowPlayerModelContainer : ScriptableObject
{
    [field: SerializeField] public FollowPlayerModel Model { get; private set; }
}
