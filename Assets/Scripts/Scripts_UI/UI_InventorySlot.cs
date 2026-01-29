using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI amountText;

    private Item currentItem;

    public void SetItem(Item item)
    {
        currentItem = item;
        //icon.sprite = item.icon;
        nameText.text = item.itemName;
    }
}