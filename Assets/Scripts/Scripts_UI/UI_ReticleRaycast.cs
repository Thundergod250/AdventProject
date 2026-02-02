using UnityEngine;

public class UI_ReticleRaycast : MonoBehaviour
{
    public Camera PlayerCamera;
    public RectTransform CrosshairUI;
    public float RayLength = 100f;
    private Ray ray;
    private Vector2 screenPos;

    //private void Awake()
    //{
    //    ray = PlayerCamera.ScreenPointToRay(screenPos);
    //    screenPos =
    //        RectTransformUtility.WorldToScreenPoint(null, CrosshairUI.position);
    //}

    private void OnDrawGizmos()
    {
        if (PlayerCamera == null || CrosshairUI == null)
            return;

        Gizmos.color = Color.blue;

        // Convert UI position → screen position
        screenPos =
            RectTransformUtility.WorldToScreenPoint(null, CrosshairUI.position);

        // Create ray from camera through crosshair
        ray = PlayerCamera.ScreenPointToRay(screenPos);
        
        Gizmos.DrawRay(ray.origin, ray.direction * RayLength);

    }

    public bool GetRaycastHit(out RaycastHit hit)
    {
        hit = new RaycastHit(); 
        
        if (PlayerCamera == null || CrosshairUI == null) return false; 
        
        if (Physics.Raycast(ray, out hit, RayLength)) 
        { 
            return true; 
        }


        return false; 
    
    }
}
