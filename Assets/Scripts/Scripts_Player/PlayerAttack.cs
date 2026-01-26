using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;

    public void OnSlash(InputAction.CallbackContext context)
    {
        if (context.performed)
            playerAnimation?.TriggerSlash();
    }
}