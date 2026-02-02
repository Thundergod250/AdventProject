using UnityEngine;

public class PlayerManipulator : MonoBehaviour
{
    // === Interaction Control ===
    public void _DisableInteraction()
    {
        var interaction = GameManager.Instance.PlayerController.PlayerInteraction;
        if (interaction != null) 
            interaction.enabled = false;
    }

    public void _EnableInteraction()
    {
        var interaction = GameManager.Instance.PlayerController.PlayerInteraction;
        if (interaction != null) 
            interaction.enabled = true;
    }

    // === Player Movement Control ===
    public void _DisablePlayerMovement() => GameManager.Instance.PlayerController?.DisableMovement();
    public void _EnablePlayerMovement()  => GameManager.Instance.PlayerController?.EnableMovement();

    // === Camera Control ===
    public void _DisableCameraMovement() => GameManager.Instance.CameraManager?.DisableCamera();
    public void _EnableCameraMovement()  => GameManager.Instance.CameraManager?.EnableCamera();

    // === Combined Movement Control ===
    public void _DisableAllMovement()
    {
        _DisablePlayerMovement();
        _DisableCameraMovement();
        _DisableInteraction();
    }

    public void _EnableAllMovement()
    {
        _EnablePlayerMovement();
        _EnableCameraMovement();
        _EnableInteraction();
    }

    // === Grab Control ===
    public void _Grab(GameObject obj)
    {
        /*var grabber = GameManager.Instance.PlayerController.PlayerGrab;
        if (grabber != null && obj != null)
        {
            var garbage = obj.GetComponent<GarbageObject>();
            if (garbage != null) 
                grabber.GrabObject(garbage);
        }*/
    }
}