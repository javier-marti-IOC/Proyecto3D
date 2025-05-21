using UnityEngine;

public class SimpleThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -12);

    [Header("Sensitivity Settings")]
    [Range(0.1f, 10f)] public float mouseSensitivity = 2f;
    [Range(1f, 500f)] public float joystickSensitivity = 50f;

    [SerializeField] private Transform player;  // El objeto del jugador (no el target)
    [SerializeField] private Vector3 followOffset = new Vector3(0, 1.6f, 0); // Donde mirar (altura)

    [Header("Invert Axis Settings")]
    public bool invertMouseY = false;
    public bool invertJoystickY = false;

    [Header("Camera Collision")]
    public LayerMask collisionMask = ~0;
    public float collisionBuffer = 0.2f;
    public float minDistanceToPlayer = 2f;

    [SerializeField] private StarterAssets.StarterAssetsInputs playerInputs;

    private Vector2 rotation = Vector2.zero;

    void Start()
    {
        
        if (target == null)
        {
            Debug.LogWarning("Falta asignar el target de la cámara.");
            return;
        }

        rotation.x = target.eulerAngles.y;
        rotation.y = target.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

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

        Quaternion cameraRotation = Quaternion.Euler(rotation.y, rotation.x, 0);
        Vector3 pivot = target.position + Vector3.up * offset.y;
        Vector3 direction = cameraRotation * Vector3.back; // Siempre hacia atrás
        float desiredDistance = -offset.z;

        Vector3 desiredPosition = pivot + direction * desiredDistance;

        if (Physics.Raycast(pivot, direction, out RaycastHit hit, desiredDistance + collisionBuffer, collisionMask))
        {
            float clampedDistance = Mathf.Clamp(hit.distance - collisionBuffer, minDistanceToPlayer, desiredDistance);
            desiredPosition = pivot + direction * clampedDistance;
        }

        transform.position = desiredPosition;
        transform.LookAt(pivot);
    }
}
