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
    public void _DisableCameraMovement() => GameManager.Instance.PlayerController?.DisableCameraMovement();
    public void _EnableCameraMovement()  => GameManager.Instance.PlayerController?.EnableCameraMovement();

    public void _DisableAnimation() => GameManager.Instance.PlayerController.GetComponent<PlayerAnimation>().SetCanAnimate(false);

    public void _EnableAnimation() => GameManager.Instance.PlayerController.GetComponent<PlayerAnimation>().SetCanAnimate(true);

    // === Combined Movement Control ===
    public void _DisableAllMovement()
    {
        Debug.Log("Player All Movement Disabled");
        _DisablePlayerMovement();
        _DisableCameraMovement();
        _DisableInteraction();
        _DisableAnimation(); 
    }

    public void _EnableAllMovement()
    {
        Debug.Log("Player All Movement Enabled");
        _EnablePlayerMovement();
        _EnableCameraMovement();
        _EnableInteraction();
        _EnableAnimation(); 
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