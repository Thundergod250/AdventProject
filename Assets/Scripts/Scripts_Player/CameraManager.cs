using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject freeLookCamera;

    public void EnableCamera()
    {
        if (freeLookCamera != null)
            freeLookCamera.gameObject.SetActive(true);
    }

    public void DisableCamera()
    {
        if (freeLookCamera != null)
            freeLookCamera.gameObject.SetActive(false);
    }
    
    public void BillboardToCamera(GameObject caller)
    {
        if (freeLookCamera == null || caller == null) return;

        // Get camera transform
        Transform camTransform = freeLookCamera.transform;

        // Rotate caller to face the camera
        Vector3 direction = camTransform.position - caller.transform.position;
        direction.y = 0f; // optional: keep billboard upright (ignore vertical tilt)

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            caller.transform.rotation = lookRotation;
        }
    }
}