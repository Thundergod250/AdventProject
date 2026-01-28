using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;


public class PlayerInventory : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject SlotPrefab;
    public GameObject InventoryStatusUI;

    public int CurrentWeight;
    public int MaxWeight;

    public void InventoryOnOpenInventory(InputAction.CallbackContext context) => GameManager.Instance.UIManager.OpenUI(UIPanelType.Inventory);

    public void AddToInventory(GarbageObject garbageObject)
    {
        if (garbageObject == null || garbageObject.ResourceData == null)
            return;

        // Add weight from ScriptableObject
        CurrentWeight += garbageObject.ResourceData.ObjectWeight;

        // Create slot
        Slot slot = Instantiate(SlotPrefab, InventoryPanel.transform).GetComponent<Slot>(); //CONVERT TO OBJECT POOLING

        // Assign UI text from ScriptableObject data
        slot.Garbage.text = garbageObject.ResourceData.ObjectName;
        slot.GarbageDescription.text = garbageObject.ResourceData.ObjectDescription;
    }

    public void ShowInventoryPanel() => InventoryPanel.SetActive(!InventoryPanel.activeSelf);

    public async void UITimerCall() => await InventoryStatusUITimer();

    public async Task InventoryStatusUITimer()
    {
        InventoryStatusUI.SetActive(true);
        await Task.Delay(2 * 1000);
        InventoryStatusUI.SetActive(false);
    }
}
