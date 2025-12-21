using UnityEngine;

public class MovementAndCameraManipulator : MonoBehaviour
{
    // === Player Movement Control ===
    public void _DisablePlayerMovement() => GameManager.Instance.PlayerController?.DisableMovement();
    public void _EnablePlayerMovement()  => GameManager.Instance.PlayerController?.EnableMovement();

    // === Camera Control ===
    public void _DisableCameraMovement() => GameManager.Instance.CameraManager?.DisableCamera();
    public void _EnableCameraMovement()  => GameManager.Instance.CameraManager?.EnableCamera();

    // === Combined Control ===
    public void _DisableAllMovement()
    {
        _DisablePlayerMovement();
        _DisableCameraMovement();
    }

    public void _EnableAllMovement()
    {
        _EnablePlayerMovement();
        _EnableCameraMovement();
    }
}