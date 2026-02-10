using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Transform slotParent; // parent transform for UI slots
    [SerializeField] private GameObject slotPrefab; // prefab for each item slot

    private void OnEnable()
    {
        if (playerInventory != null)
            playerInventory.EvtOnInventoryChanged.AddListener(UpdateUI);
    }

    private void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.EvtOnInventoryChanged.RemoveListener(UpdateUI);
    }

    public void UpdateUI()
    {
        // Clear old slots
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        // Create new slots
        foreach (var item in playerInventory.items)
        {
            var slot = Instantiate(slotPrefab, slotParent);
            var slotUI = slot.GetComponent<UI_InventorySlot>();
            if (slotUI != null)
                slotUI.SetItem(item);
        }
    }
}