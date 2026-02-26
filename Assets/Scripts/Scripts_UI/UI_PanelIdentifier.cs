using UnityEngine;

public enum UIPanelType
{
    None,
    MainUI,
    Inventory,
    Shop1,
    Gacha,
    Mine,
    Monolith,
    BlackCanvas
}

public class UIPanelIdentifier : MonoBehaviour
{
    public UIPanelType PanelType;
}