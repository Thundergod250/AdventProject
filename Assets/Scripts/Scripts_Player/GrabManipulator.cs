using UnityEngine;

public class GrabManipulator : MonoBehaviour
{
    public void _Grab(GameObject obj) => GameManager.Instance.PlayerController.PlayerGrab?.GrabObject(obj);

    public void _Release() => GameManager.Instance.PlayerController.PlayerGrab?.ReleaseGrabbedObject();

    public void _Remove() => GameManager.Instance.PlayerController.PlayerGrab?.RemoveGrabObject();
}