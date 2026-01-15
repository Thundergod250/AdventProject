using UnityEngine;

public class GarbageObject : MonoBehaviour
{
    [Header("Garbage Data Reference")]
    public ResourceData ResourceData;

    // Example usage
    private void Start()
    {
        if (ResourceData != null)
        {
            Debug.Log($"Garbage: {ResourceData.ObjectName}, Group: {ResourceData.Group}, Weight: {ResourceData.ObjectWeight}");
        }
    }
}