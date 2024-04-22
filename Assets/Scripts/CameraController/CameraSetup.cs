using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    [Header("Follow Player")]
    [SerializeField] private FollowPlayerModelContainer followPlayerModelContainer;
    [SerializeField] private FollowPlayer followPlayer;

    private void OnEnable()
    {
        if (followPlayer && followPlayerModelContainer)
        {
            followPlayer.Model = followPlayerModelContainer.Model;
            followPlayer.enabled = true;
        }
    }
}
