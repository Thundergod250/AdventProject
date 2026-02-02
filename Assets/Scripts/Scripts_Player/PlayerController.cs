using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public PlayerAnimation PlayerAnimation;
    public PlayerInteraction PlayerInteraction;
    public PlayerGrab PlayerGrab;
    public PlayerInventory PlayerInventory;
    public PlayerAttack PlayerAttack;
    public PlayerInput PlayerInput;
    public Animator animator;

    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.PlayerController = this;
    }

    // === Input System Callbacks ===
    public void OnMove(InputAction.CallbackContext context)
    {
        PlayerMovement.MovementOnMove(context); 
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        PlayerMovement.MovementOnJump(context);
    }
    
    public void OnLookJump(InputAction.CallbackContext context)
    {
        PlayerMovement.MovementOnLook(context);
    }


    public void OnOpeninventory(InputAction.CallbackContext context)
    {
        PlayerInventory.InventoryOnOpenInventory(context);
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        PlayerAttack.OnSlash(context);
    }

    // === Movement Control Methods ===
    public void EnableMovement()
    {
        //PlayerMovement.SetCanMove(true);
    }

    public void DisableMovement()
    {
        //PlayerMovement.DisableMovement();
    }
}
