using Unity.Cinemachine;
using UnityEngine;

public class FreeLookCamControl : MonoBehaviour
{
    public CinemachineCamera virtualCamera; // Assign your Vcam here in the Inspector

    // Call this method to stop movement
    public void DisableCameraMovement()
    {
        if (virtualCamera != null)
        {
            virtualCamera.enabled = false;
        }
    }

    // Call this method to re-enable movement
    public void EnableCameraMovement()
    {
        if (virtualCamera != null)
        {
            virtualCamera.enabled = true;
        }
    }
}
