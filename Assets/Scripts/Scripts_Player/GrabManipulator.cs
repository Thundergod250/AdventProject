using UnityEngine;

public class GrabManipulator : MonoBehaviour
{
    public void _Grab(GameObject obj) => GameManager.Instance.PlayerController.PlayerGrab?.GrabObject(obj);
}