using UnityEngine;

public class UI_Grab_Tab : MonoBehaviour
{
    [SerializeField] private GameObject grabTab;

    public void EnableGrabTab() => grabTab.SetActive(true);

    public void DisableGrabTab() => grabTab.SetActive(false);
}
