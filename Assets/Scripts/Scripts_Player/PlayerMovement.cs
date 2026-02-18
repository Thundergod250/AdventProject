using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    [SerializeField] private float jumpSpeedMultiplier = 1.5f;

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

    // === NoClip ===
    private bool noClipEnabled = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        HandleGroundCheck();

        if (noClipEnabled)
        {
            HandleNoClipMovement();
        }
        else if (canMove)
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

        if (noClipEnabled)
        {
            // Space = ascend
            if (context.performed)
                velocity.y = moveSpeed;
            else if (context.canceled)
                velocity.y = 0f;
        }
        else
        {
            if (context.performed)
                jumpRequested = true;
            else if (context.canceled)
                jumpRequested = false;
        }
    }

    public void MovementOnCtrl(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        if (noClipEnabled)
        {
            // Ctrl = descend
            if (context.performed)
                velocity.y = -moveSpeed;
            else if (context.canceled)
                velocity.y = 0f;
        }
    }

    public void MovementOnNoClip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            noClipEnabled = !noClipEnabled;

            controller.enabled = !noClipEnabled; // disable collider when NoClip
            velocity = Vector3.zero; // reset velocity
        }
    }

    // === Control toggles ===
    public void SetCanMove(bool value) => canMove = value;
    public bool GetCanMove() => canMove;
    public bool GetCanLook() => canLook;

    public void SetCanLook(bool value)
    {
        canLook = value;
        if (lookController)
            lookController.enabled = value;
    }

    // === Internal Logic ===
    private void HandleGroundCheck()
    {
        if (noClipEnabled) { isGrounded = false; return; }

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

    private void HandleNoClipMovement()
    {
        if (!cameraTransform) return;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = camRight * moveInput.x + camForward * moveInput.y;

        Vector3 finalMove = move * moveSpeed + new Vector3(0f, velocity.y, 0f);
        transform.position += finalMove * Time.deltaTime;

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public bool IsGrounded() => isGrounded;

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
