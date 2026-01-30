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

    [Header("Ground Check")]
    public Transform groundCheck;          // Empty GameObject at feet
    public float groundRadius = 0.3f;      // Radius of overlap sphere
    public LayerMask groundMask;           // Layers considered "ground"

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpRequested;

    private void Awake() => controller = GetComponent<CharacterController>();

    private void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleLook();
        HandleGravityAndJump();
    }

    // === Called from PlayerController ===
    public void MovementOnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

    public void MovementOnLook(InputAction.CallbackContext context) => lookInput = context.ReadValue<Vector2>();

    public void MovementOnJump(InputAction.CallbackContext context)
    {
        if (context.performed) 
            jumpRequested = true;
    }

    // === Internal Logic ===
    private void HandleGroundCheck()
    {
        // Sphere check at feet
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0) 
            velocity.y = -2f; // small downward force to keep grounded
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * mouseX);
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
