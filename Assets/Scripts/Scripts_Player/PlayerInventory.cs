using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> Item = new List<GameObject>();
    public GameObject PlayerInventoryUI;

    public void InventoryOnOpenInventory(InputAction.CallbackContext context)
    {
        PlayerInventoryUI.SetActive(!PlayerInventoryUI.activeSelf);

    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.I))
        //    PlayerInventoryUI.SetActive(!PlayerInventoryUI.activeSelf);
    }
}
