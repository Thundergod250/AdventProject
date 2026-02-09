using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimation playerAnimation;

    public void OnSlam(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Disable movement
        playerMovement.SetCanMove(false);

        // Trigger slam animation
        StartCoroutine(SlamRoutine());
    }

    private IEnumerator SlamRoutine()
    {
        yield return StartCoroutine(playerAnimation.PlaySlam());

        // Re-enable movement after slam finishes
        playerMovement.SetCanMove(true);
    }
}