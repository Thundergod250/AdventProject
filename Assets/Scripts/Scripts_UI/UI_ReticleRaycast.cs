using UnityEngine;

public class UI_ReticleRaycast : MonoBehaviour
{
    public Camera PlayerCamera;
    public RectTransform CrosshairUI;
    public float RayLength = 100f;

    private void OnDrawGizmos()
    {
        if (PlayerCamera == null || CrosshairUI == null)
            return;

        Gizmos.color = Color.blue;

        // Convert UI position → screen position
        Vector2 screenPos =
            RectTransformUtility.WorldToScreenPoint(null, CrosshairUI.position);

        // Create ray from camera through crosshair
        Ray ray = PlayerCamera.ScreenPointToRay(screenPos);

        Gizmos.DrawRay(ray.origin, ray.direction * RayLength);
    }
}
