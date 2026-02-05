using UnityEngine;

public class InteractionManipulator : MonoBehaviour
{
    public void _DisableInteraction()
    {
        if (GameManager.Instance.PlayerController.UI_ReticleRaycast != null) 
            GameManager.Instance.PlayerController.UI_ReticleRaycast.enabled = false;
    }

    public void _EnableInteraction()
    {
        if (GameManager.Instance.PlayerController.UI_ReticleRaycast != null) 
            GameManager.Instance.PlayerController.UI_ReticleRaycast.enabled = true;
    }
}