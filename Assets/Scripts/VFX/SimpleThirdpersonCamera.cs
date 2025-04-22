using UnityEngine;

public class SimpleThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float mouseSensitivity = 2f;
    public float rotationSmoothTime = 0.1f;
    public float distanceDamping = 5f;
    public float recentreDelay = 1.0f;
    public float recentreSpeed = 2.0f;

    [Header("Camera Collision")]
    public LayerMask collisionMask = ~0; // Por defecto todas las capas
    public float collisionBuffer = 0.2f; // Distancia para no pegarse tanto

    private Vector2 rotation = Vector2.zero;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 currentPosition;

    private float lastMouseInputTime;
    private bool isRecentering = false;

    [Header("Vertical Recentering")]
    [SerializeField] private float verticalRecentreDelay = 1f;
    [SerializeField] private float verticalRecentreSpeed = 2f;
    [SerializeField] private float defaultVerticalAngle = 20f;

    private float verticalRecentreTimer = 0f;

    void Start()
    {
        if (target == null)
            Debug.LogWarning("¡Falta asignar el target de la cámara!");

        rotation.x = transform.eulerAngles.y;
        rotation.y = transform.eulerAngles.x;
        currentPosition = transform.position;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        bool hasMouseInput = Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f;

        if (hasMouseInput)
        {
            lastMouseInputTime = Time.time;
            isRecentering = false;
            rotation.x += mouseX * mouseSensitivity;
            rotation.y -= mouseY * mouseSensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -35f, 70f);
        }

        // Vertical auto recentre
        if (Mathf.Abs(mouseY) > 0.01f)
        {
            verticalRecentreTimer = 0f;
        }
        else
        {
            verticalRecentreTimer += Time.deltaTime;
            if (verticalRecentreTimer > verticalRecentreDelay)
            {
                rotation.y = Mathf.Lerp(rotation.y, defaultVerticalAngle, Time.deltaTime * verticalRecentreSpeed);
            }
        }

        // Recentrado horizontal
        bool isPlayerMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

        if (isPlayerMoving || (!hasMouseInput && Time.time - lastMouseInputTime > recentreDelay))
        {
            isRecentering = true;
        }

        if (isRecentering)
        {
            float targetYaw = target.eulerAngles.y;
            rotation.x = Mathf.LerpAngle(rotation.x, targetYaw, Time.deltaTime * recentreSpeed);
        }

        // Cálculo de posición
        Quaternion targetRotation = Quaternion.Euler(rotation.y, rotation.x, 0);
        Vector3 desiredPosition = target.position + targetRotation * offset;

        // 🔍 DETECCIÓN DE PAREDES
        Vector3 origin = target.position + Vector3.up * 1.5f; // punto de origen del raycast
        Vector3 direction = (desiredPosition - origin).normalized;
        float maxDistance = offset.magnitude;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance + collisionBuffer, collisionMask))
        {
            // Si golpea algo, colocamos la cámara justo delante del obstáculo
            desiredPosition = hit.point - direction * collisionBuffer;
        }

        currentPosition = Vector3.Lerp(currentPosition, desiredPosition, Time.deltaTime * distanceDamping);
        transform.position = currentPosition;

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
