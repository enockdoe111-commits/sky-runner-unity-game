// CameraFollow.cs - Smooth third-person camera that follows the player

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset = new Vector3(0, 3, -5);
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float minY = 2f;
    [SerializeField] private float maxY = 10f;

    private Vector3 targetPosition;
    private Rigidbody playerRb;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        playerRb = playerTransform.GetComponent<Rigidbody>();

        if (playerTransform != null)
        {
            targetPosition = playerTransform.position + offset;
            transform.position = targetPosition;
        }
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        // Calculate target position
        targetPosition = playerTransform.position + offset;

        // Clamp Y position for better camera angle
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        // Smoothly move camera to target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // Look at a point slightly above the player
        transform.LookAt(playerTransform.position + Vector3.up * 1f);
    }
}
