using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName;
    public int amount = 1;

    public virtual bool IsSameType(Item other) => itemName == other.itemName;
}