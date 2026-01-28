using UnityEngine;
using UnityEngine.Events;

public class PlayerGrab : MonoBehaviour
{
    public bool IsPlayerCarryingObject;

    // === Events ===
    public UnityEvent<GameObject> EvtOnGrab;
    public UnityEvent<GameObject> EvtOnReleaseGrabObj;
    public UnityEvent<GameObject> EvtOnRemovedGrabbedObject;

    private GameObject currentGrabbedObj;

    public void GrabObject(GarbageObject obj)
    {
        if (GameManager.Instance.PlayerInventory.CurrentWeight < GameManager.Instance.PlayerInventory.MaxWeight)
        {
            GameManager.Instance.PlayerInventory.AddToInventory(obj);
            Destroy(obj.gameObject); // TO CONVERT TO OBJECT POOLING

            // 🔑 Trigger grab animation
            GameManager.Instance.PlayerController.PlayerAnimation?.TriggerGrab();
        }
        else
        {
            GameManager.Instance.PlayerInventory.UITimerCall();
            Debug.LogWarning("No More Space");
        }
    }
}