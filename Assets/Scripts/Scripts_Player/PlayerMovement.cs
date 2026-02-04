using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    private float pitch;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpRequested;

    // 👇 Control flags
    private bool canMove = true;
    private bool canLook = true;

    private void Awake() => controller = GetComponent<CharacterController>();

    private void Update()
    {
        HandleGroundCheck();

        if (canMove)
        {
            HandleMovement();
            HandleGravityAndJump();
        }

        if (canLook)
        {
            HandleLook();
            HandleLookVertical();
        }
    }

    // === Called from PlayerController ===
    public void MovementOnMove(InputAction.CallbackContext context)
    {
        if (canMove) moveInput = context.ReadValue<Vector2>();
        else moveInput = Vector2.zero;
    }

    public void MovementOnLook(InputAction.CallbackContext context)
    {
        if (canLook) lookInput = context.ReadValue<Vector2>();
        else lookInput = Vector2.zero;
    }

    public void MovementOnJump(InputAction.CallbackContext context)
    {
        if (canMove && context.performed) 
            jumpRequested = true;
    }

    // === Control toggles ===
    public void SetCanMove(bool value) => canMove = value;
    public void SetCanLook(bool value) => canLook = value;

    // === Internal Logic ===
    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        // Horizontal rotation (yaw) — rotates the player body
        float mouseX = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleLookVertical()
    {
        // Vertical rotation (pitch) — rotates only the camera pivot
        float mouseY = lookInput.y * lookSensitivity;

        pitch -= mouseY; // subtract to invert standard FPS controls
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 👇 Only rotate the camera pivot, not the body
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void HandleGravityAndJump()
    {
        if (isGrounded && jumpRequested)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
