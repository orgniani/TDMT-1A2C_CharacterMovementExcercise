using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offsetUp = 2f;
    [SerializeField] private float speed = 10;

    [SerializeField] private float distance = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60f;

    private float currentX = 0f;
    private float currentY = 0f;

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

        Vector3 offset = Vector3.up * offsetUp;

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position + offset;

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }

    public void SetInputRotation(Vector2 input)
    {
        currentX += input.x * speed * Time.deltaTime;
        currentY -= input.y * rotationSpeed * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);
    }
}
