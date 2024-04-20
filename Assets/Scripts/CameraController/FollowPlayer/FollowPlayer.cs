using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float currentX = 0f;
    private float currentY = 0f;

    public FollowPlayerModel Model { get; set; }

    private void Awake()
    {
        if (!target)
        {
            Debug.LogError($"{name}: Target is null!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (!target)
            return;

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        Vector3 offset = Vector3.up * Model.OffsetUp;

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -Model.Distance);
        Vector3 position = rotation * negDistance + target.position + offset;

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Model.RotationSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, position, Model.Speed * Time.deltaTime);
    }

    public void SetInputRotation(Vector2 input)
    {
        currentX += input.x * Model.Sensitivity * Time.deltaTime;
        currentY -= input.y * Model.Sensitivity * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, Model.MinVerticalAngle, Model.MaxVerticalAngle);
    }
}
