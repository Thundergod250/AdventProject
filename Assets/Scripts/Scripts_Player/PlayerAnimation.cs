using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Normal States")]
    [SerializeField] private string idleState = "Idle";
    [SerializeField] private string runState = "Walk";
    [SerializeField] private string jumpState = "Jump";

    [Header("Action States")]
    [SerializeField] private string grabState = "Basic Grab";
    [SerializeField] private string slashState = "Slash";

    private string currentState;

    private void PlayState(string stateName, float crossFade = 0.05f)
    {
        if (animator == null || currentState == stateName) return;

        animator.CrossFade(stateName, crossFade, 0);
        currentState = stateName;
    }

    public void UpdateMovementAnimation(float speed, bool isJumping)
    {
        if (isJumping) return;

        float blend = 0.05f;
        PlayState(speed > 0.1f ? runState : idleState, blend);
    }

    public void TriggerJump() => PlayState(jumpState, 0.05f);

    public void TriggerGrab() => PlayState(grabState, 0.1f);

    public void TriggerSlash() => PlayState(slashState, 0.1f);

    // Called via Animation Event at end of Slash animation
    public void OnSlashAnimationEnd()
    {
        currentState = null; // allow movement to take over again
    }

    public void ResetAnimations() => PlayState(idleState, 0.1f);
}