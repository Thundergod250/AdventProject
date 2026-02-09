using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject gameCamera;

    public void EnableCamera()
    {
        if (gameCamera != null)
            gameCamera.SetActive(true);
    }

    public void DisableCamera()
    {
        if (gameCamera != null)
            gameCamera.SetActive(false);
    }

    public void BillboardToCamera(GameObject caller)
    {
        if (caller == null || gameCamera == null) return;

        Transform camTransform = gameCamera.transform;

        // Make caller face the camera
        caller.transform.forward = camTransform.forward;
    }
}