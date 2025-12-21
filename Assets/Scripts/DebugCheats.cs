using UnityEngine;
using UnityEngine.InputSystem;

public class DebugCheats : MonoBehaviour
{
    private bool noclipEnabled = false;

    public void OnNoclipToggle(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        noclipEnabled = !noclipEnabled;
        Debug.Log("Noclip toggled: " + noclipEnabled);

        // Hook into PlayerMovement
        if (GameManager.Instance != null && GameManager.Instance.PlayerController != null)
        {
            GameManager.Instance.PlayerController.PlayerMovement.SetNoclip(noclipEnabled);
        }
    }
}