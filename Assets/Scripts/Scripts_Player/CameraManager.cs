using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject firstPersonCamera;

    public void EnableCamera()
    {
        if (firstPersonCamera != null)
            firstPersonCamera.SetActive(true);
    }

    public void DisableCamera()
    {
        if (firstPersonCamera != null)
            firstPersonCamera.SetActive(false);
    }

    public void BillboardToCamera(GameObject caller)
    {
        /*if (firstPersonCamera == null || caller == null) return;

        Transform camTransform = firstPersonCamera.transform;
        Vector3 direction = camTransform.position - caller.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            caller.transform.rotation = lookRotation;
        }*/
    }
}