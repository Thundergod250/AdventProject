using UnityEngine;

public enum UIPanelType
{
    None,
    MainUI,
    Inventory,
    Shop1,
    Gacha,
}

public class UIPanelIdentifier : MonoBehaviour
{
    public UIPanelType PanelType;
}