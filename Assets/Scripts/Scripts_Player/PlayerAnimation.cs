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

        if (speed > 0.1f)
            PlayState(runState);
        else
            PlayState(idleState);
    }

    public void TriggerJump() => PlayState(jumpState, 0.05f);

    public void TriggerGrab() => PlayState(grabState, 0.1f);

    public void TriggerSlash() => PlayState(slashState, 0.1f);

    public void ResetAnimations() => PlayState(idleState, 0.1f);
}