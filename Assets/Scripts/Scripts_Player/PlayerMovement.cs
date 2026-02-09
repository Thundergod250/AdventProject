using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundMask;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform; // reference to Cinemachine camera
    public CinemachineInputAxisController lookController; // controls camera orbit
    public float rotationSpeed = 10f; // how quickly player turns

    [Header("Animation")]
    public PlayerAnimation playerAnimation; // reference to slime bounce animation

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector2 moveInput;
    private bool jumpRequested;

    // Control flags
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
    }

    // === Input Callbacks ===
    public void MovementOnMove(InputAction.CallbackContext context)
    {
        if (canMove) moveInput = context.ReadValue<Vector2>();
        else moveInput = Vector2.zero;
    }

    public void MovementOnJump(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        if (context.performed)
            jumpRequested = true;
        else if (context.canceled)
            jumpRequested = false;
    }

    // === Control toggles ===
    public void SetCanMove(bool value) => canMove = value;

    public void SetCanLook(bool value)
    {
        canLook = value;
        if (lookController != null)
            lookController.enabled = value; // enable/disable Cinemachine orbit input
    }

    // === Internal Logic ===
    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;
    }

    private void HandleMovement()
    {
        if (cameraTransform == null) return;

        // Get camera forward/right projected onto ground plane
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Movement relative to camera
        Vector3 move = camRight * moveInput.x + camForward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Smoothly rotate player to face movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // 🔔 Trigger animation bounce only when grounded
        bool isActuallyMoving = move.magnitude > 0.1f && isGrounded;
        if (playerAnimation != null)
            playerAnimation.SetIsMoving(isActuallyMoving);
    }

    private void HandleGravityAndJump()
    {
        if (isGrounded)
        {
            if (jumpRequested)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpRequested = false;
            }
            else if (velocity.y < 0)
                velocity.y = -2f;
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
