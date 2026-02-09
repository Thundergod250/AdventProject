using UnityEngine;
using UnityEngine.Events;

public class PlayerGrab : MonoBehaviour
{
    private PlayerInventory inventory;

    private void Start()
    {
        inventory = GameManager.Instance.PlayerController.PlayerInventory;
    }

    /*public void GrabObject(Item item)
    {
        if (item == null) return;

        inventory.AddItem(item);

        // Destroy the world object after pickup
        Destroy(item.gameObject);
    }*/
}