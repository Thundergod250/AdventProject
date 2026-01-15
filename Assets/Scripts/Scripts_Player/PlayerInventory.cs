using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;


public class PlayerInventory : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject SlotPrefab;
    public GameObject PlayerInventoryUI;
    public GameObject InventoryStatusUI;

    public int CurrentWeight;
    public int MaxWeight;

    public void InventoryOnOpenInventory(InputAction.CallbackContext context) => PlayerInventoryUI.SetActive(!PlayerInventoryUI.activeSelf);

    public void AddToInventory(GarbageObject garbageObject)
    {
        CurrentWeight += garbageObject.ObjectWeight;

        Slot slot = Instantiate(SlotPrefab, InventoryPanel.transform).GetComponent<Slot>();


        if(garbageObject != null)
        {
            slot.Garbage.text = garbageObject.ObjectName;
            slot.GarbageDescription.text = garbageObject.ObjectDescription;
        }
    }

    public void ShowInventoryPanel() => InventoryPanel.SetActive(!InventoryPanel.activeSelf);

    public async void UITimerCall() => await InventoryStatusUITimer();

    public async Task InventoryStatusUITimer()
    {
        InventoryStatusUI.SetActive(true);
        await Task.Delay(2 * 1000);
        InventoryStatusUI.SetActive(false);
    }

    //public bool AddItem(GameObject itemPrefab)
    //{
    //    foreach(Transform SlotTransform in InventoryPanel.transform)
    //    {
    //        Slot slot = SlotTransform.GetComponent<Slot>();

    //        if(slot != null && slot.currentItem == null)
    //        {
    //            GameObject newItem = Instantiate(itemPrefab, SlotTransform);
    //            newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    //            slot.currentItem = newItem;
    //            return true;
    //        }
    //    }

    //    return false;
    //}
}
