using System;
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
    [SerializeField] private float jumpSpeedMultiplier = 1.5f; // ✅ editable boost

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundMask;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    public CinemachineInputAxisController lookController;
    public float rotationSpeed = 10f;

    private PlayerAnimation playerAnimation;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector2 moveInput;
    private bool jumpRequested;

    private bool canMove = true;
    private bool canLook = true;

    private void Start()
    { 
        controller = GetComponent<CharacterController>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }


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
            lookController.enabled = value;
    }

    // === Internal Logic ===
    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;
    }

    private void HandleMovement()
    {
        if (!cameraTransform) return;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = camRight * moveInput.x + camForward * moveInput.y;

        // ✅ Apply jump speed boost if airborne
        float currentSpeed = isGrounded ? moveSpeed : moveSpeed * jumpSpeedMultiplier;
        controller.Move(move * (currentSpeed * Time.deltaTime));

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        bool isActuallyMoving = move.magnitude > 0.1f && isGrounded;
        if (playerAnimation)
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

    public bool IsGrounded() => isGrounded; // ✅ expose grounded state

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
