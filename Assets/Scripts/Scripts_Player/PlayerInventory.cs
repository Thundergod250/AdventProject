using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject SlotPrefab;
    public int SlotCount;
    public List<GameObject> Item = new List<GameObject>();
    public GameObject PlayerInventoryUI;

    public void InventoryOnOpenInventory(InputAction.CallbackContext context)
    {
        PlayerInventoryUI.SetActive(!PlayerInventoryUI.activeSelf);
    }

    public void AddToInventory(GameObject ItemObject)
    {
        Item.Add(ItemObject);
        Slot slot = Instantiate(SlotPrefab, InventoryPanel.transform).GetComponent<Slot>();

        GarbageObject garbageObject = ItemObject.GetComponent<GarbageObject>();

        if(garbageObject != null)
        {
            slot.Garbage.text = garbageObject.ObjectName;
            slot.GarbageDescription.text = garbageObject.ObjectDescription;
        }

    }

    public void ShowInventoryPanel()
    {
        InventoryPanel.SetActive(!InventoryPanel.activeSelf);
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
