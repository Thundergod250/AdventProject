using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> items = new();

    [Header("Inventory Events")]
    public UnityEvent EvtOnInventoryChanged; 

    public void InventoryOnOpenInventory(InputAction.CallbackContext context) => GameManager.Instance.UIManager.ToggleUI(UIPanelType.Inventory);
    
    public void AddItem(Item item)
    {
        items.Add(item);
        Debug.Log($"Added {item.itemName} x{item.amount} to inventory.");
        EvtOnInventoryChanged?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Removed {item.itemName} from inventory.");
            EvtOnInventoryChanged?.Invoke();
        }
    }
}