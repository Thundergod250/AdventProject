using UnityEngine;

public enum UIPanelType
{
    None,
    MainUI,
    Inventory,
    Shop1,
    Gacha,
    Mine,
    BlackCanvas
}

public class UIPanelIdentifier : MonoBehaviour
{
    public UIPanelType PanelType;
}