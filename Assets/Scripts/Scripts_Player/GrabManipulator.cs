using UnityEngine;

public class GrabManipulator : MonoBehaviour
{
    public void _Grab(GameObject obj)
    {
        var item = obj.GetComponent<Item>();
        if (item != null)
        {
            GameManager.Instance.PlayerController.PlayerGrab?.GrabObject(item);
        }
    }
}