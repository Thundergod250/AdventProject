using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public void OnSlash(InputAction.CallbackContext context)
    {
        if (context.performed)
            GameManager.Instance.PlayerController.PlayerAnimation?.TriggerSlash();
    }
}