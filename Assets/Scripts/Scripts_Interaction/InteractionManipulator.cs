using UnityEngine;

public class InteractionManipulator : MonoBehaviour
{
    public void _DisableInteraction()
    {
        if (GameManager.Instance.PlayerController.PlayerInteraction != null)
        {
            GameManager.Instance.UIManager.UI_Grab_Tab.EnableGrabTab();
            GameManager.Instance.PlayerController.PlayerInteraction.enabled = false;
        }
    }

    public void _EnableInteraction()
    {
        if (GameManager.Instance.PlayerController.PlayerInteraction != null)
        {
            GameManager.Instance.UIManager.UI_Grab_Tab.DisableGrabTab();
            GameManager.Instance.PlayerController.PlayerInteraction.enabled = true;
        }
    }
}