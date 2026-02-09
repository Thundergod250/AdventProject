using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimation playerAnimation;

    private bool isAttacking = false;

    public void OnSlam(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isAttacking) return; // prevent re-entry
        if (!playerMovement.IsGrounded()) return; // ✅ only attack if grounded

        playerMovement.SetCanMove(false);
        StartCoroutine(SlamRoutine());
    }

    private IEnumerator SlamRoutine()
    {
        isAttacking = true;
        yield return StartCoroutine(playerAnimation.PlaySlam());
        playerMovement.SetCanMove(true);
        isAttacking = false;
    }
}