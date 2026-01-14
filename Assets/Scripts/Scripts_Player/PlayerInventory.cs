using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> Item = new List<GameObject>();
    public GameObject PlayerInventoryUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
            PlayerInventoryUI.SetActive(!PlayerInventoryUI.activeSelf);
    }
}
