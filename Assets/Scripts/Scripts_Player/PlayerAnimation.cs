using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Normal States")]
    [SerializeField] private string idleState = "Idle";
    [SerializeField] private string runState = "Walk";
    [SerializeField] private string jumpState = "Jump";

    [Header("Grab States")]
    [SerializeField] private string grabIdleState = "Basic Grab";
    [SerializeField] private string grabWalkState = "Grab and Walk";
    [SerializeField] private string grabJumpState = "Grab and Jump";

    private PlayerGrab playerGrab;
    private string currentState;

    private void Start()
    {
        playerGrab = GetComponent<PlayerGrab>();
    }

    private void PlayState(string stateName, float crossFade = 0.15f)
    {
        if (animator == null) return;
        if (currentState == stateName) return;

        animator.CrossFade(stateName, crossFade, 0);
        currentState = stateName;
    }

    public void UpdateMovementAnimation(float speed, bool isJumping)
    {
        if (isJumping) return;

        bool isCarrying = playerGrab != null && playerGrab.IsPlayerCarryingObject;

        if (speed > 0.1f)
        {
            PlayState(isCarrying ? grabWalkState : runState);
        }
        else
        {
            PlayState(isCarrying ? grabIdleState : idleState);
        }
    }

    public void TriggerJump()
    {
        bool isCarrying = playerGrab != null && playerGrab.IsPlayerCarryingObject;
        PlayState(isCarrying ? grabJumpState : jumpState, 0.05f);
    }

    public void ResetAnimations()
    {
        bool isCarrying = playerGrab != null && playerGrab.IsPlayerCarryingObject;
        PlayState(isCarrying ? grabIdleState : idleState, 0.1f);
    }
}