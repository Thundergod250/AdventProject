using UnityEngine;
using UnityEngine.Events;

public class PlayerGrab : MonoBehaviour
{
    public bool IsPlayerCarryingObject;

    // === Events ===
    public UnityEvent<GameObject> EvtOnGrab;
    public UnityEvent<GameObject> EvtOnReleaseGrabObj;
    public UnityEvent<GameObject> EvtOnRemovedGrabbedObject;

    [Header("Grab Settings")]
    [SerializeField] private float tossForce = 5f;
    [SerializeField] private float grabDelay = 2f;

    [Header("Downward Ramp Settings")]
    [SerializeField] private float downwardForceMultiplier = 0f; // starting downward force
    [SerializeField] private float downwardRampRate = 5f;        // how fast it increases

    private GameObject currentGrabbedObj;
    private bool isOnCooldown = false;

    public void GrabObject(GarbageObject obj)
    {
        if(GameManager.Instance.PlayerInventory.CurrentWeight < GameManager.Instance.PlayerInventory.MaxWeight)
        {
            GameManager.Instance.PlayerInventory.AddToInventory(obj);
            Destroy(obj.gameObject); // TO CONVERT TO OBJECT POOLING
        }
        else
        {
            GameManager.Instance.PlayerInventory.UITimerCall();
            //GameManager.Instance.PlayerInventory.InventoryStatusUI.SetActive(!GameManager.Instance.PlayerInventory.InventoryStatusUI.activeSelf);
            Debug.LogWarning("No More Space");
        }
    }
}
