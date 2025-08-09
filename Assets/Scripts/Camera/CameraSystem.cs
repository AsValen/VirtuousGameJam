using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Camera mainCamera;
    [SerializeField] private float followSpeed = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("Camera component not found!");
            return;
        }

        mainCamera.orthographic = true;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player == null)
        {
            Debug.LogWarning("Player not found! Camera will not follow.");
            return;
        }

        Vector3 desiredPosition = player.position;
        Vector3 currentPosition = transform.position;
        Vector3 direction = (desiredPosition - currentPosition).normalized;

        float distance = Vector3.Distance(currentPosition, desiredPosition);
        float maxMove = followSpeed * Time.fixedDeltaTime;
        Vector3 moveDelta = direction * Mathf.Min(distance, maxMove);

        Vector3 newPosition = currentPosition + moveDelta;

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}
