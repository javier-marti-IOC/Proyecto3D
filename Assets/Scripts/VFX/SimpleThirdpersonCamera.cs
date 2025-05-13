using UnityEngine;

public class SimpleThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);

    [Header("Sensitivity Settings")]
    [Range(0.1f, 10f)] public float mouseSensitivity = 2f;
    [Range(1f, 500f)] public float joystickSensitivity = 50f;

    [Header("Invert Axis Settings")]
    public bool invertMouseY = false;
    public bool invertJoystickY = false;

    [Header("Other Settings")]
    public float distanceDamping = 5f;

    [Header("Camera Collision")]
    public LayerMask collisionMask = ~0;
    public float collisionBuffer = 0.2f;

    [SerializeField] private StarterAssets.StarterAssetsInputs playerInputs;

    private Vector2 rotation = Vector2.zero;
    private Vector3 currentPosition;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Falta asignar el target de la cámara.");
            return;
        }

        rotation.x = target.eulerAngles.y;
        rotation.y = target.eulerAngles.x;

        Quaternion initialRotation = Quaternion.Euler(rotation.y, rotation.x, 0);
        currentPosition = target.position + initialRotation * offset;

        Cursor.lockState = CursorLockMode.Locked;

        // Colocar cámara al inicio
        Vector3 origin = target.position + Vector3.up * 1.5f;
        Vector3 direction = (currentPosition - origin).normalized;
        float maxDistance = offset.magnitude;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance + collisionBuffer, collisionMask))
        {
            currentPosition = hit.point - direction * collisionBuffer;
        }

        transform.position = currentPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    void LateUpdate()
    {
        if (target == null) return;

        float inputX = playerInputs.look.x;
        float inputY = playerInputs.look.y;

        bool hasLookInput = Mathf.Abs(inputX) > 0.01f || Mathf.Abs(inputY) > 0.01f;
        bool isUsingMouse = playerInputs.cursorInputForLook;

        if (hasLookInput)
        {
            if (isUsingMouse)
            {
                float mouseY = invertMouseY ? -inputY : inputY;
                rotation.x += inputX * mouseSensitivity;
                rotation.y += mouseY * mouseSensitivity;
            }
            else
            {
                float joyY = invertJoystickY ? inputY : -inputY;
                rotation.x += inputX * joystickSensitivity * Time.deltaTime;
                rotation.y += joyY * joystickSensitivity * Time.deltaTime;
            }

            rotation.y = Mathf.Clamp(rotation.y, -35f, 70f);
        }

        // Posicionamiento de la cámara
        Quaternion targetRotation = Quaternion.Euler(rotation.y, rotation.x, 0);
        Vector3 desiredPosition = target.position + targetRotation * offset;

        Vector3 origin = target.position + Vector3.up * 1.5f;
        Vector3 direction = (desiredPosition - origin).normalized;
        float maxDistance = offset.magnitude;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance + collisionBuffer, collisionMask))
        {
            desiredPosition = hit.point - direction * collisionBuffer;
        }

        //currentPosition = Vector3.Lerp(currentPosition, desiredPosition, Time.deltaTime * distanceDamping);
        currentPosition = Vector3.SmoothDamp(currentPosition, desiredPosition, ref velocity, 1f / distanceDamping);
        transform.position = currentPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
