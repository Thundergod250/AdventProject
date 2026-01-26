using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_TowerShop : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform cardParent;

    [Header("Shop Buttons")]
    [SerializeField] private GameObject towerUpgradesButton;
    [SerializeField] private GameObject offensiveButton;
    [SerializeField] private GameObject defensiveButton;
    [SerializeField] private GameObject utilityButton;
    
    private Dictionary<string, GameObject> shopButtons;
    private readonly List<GameObject> activeCards = new();

    private void Awake()
    {
        shopButtons = new Dictionary<string, GameObject>
        {
            { "Upgrades", towerUpgradesButton },
            { "Offensive", offensiveButton },
            { "Defensive", defensiveButton },
            { "Utility", utilityButton }
        };
    }

    // === Button visibility ===
    public void ShowShopButtons(bool showUpgrades)
    {
        shopButtons["Upgrades"].SetActive(showUpgrades);
        shopButtons["Offensive"].SetActive(true);
        shopButtons["Defensive"].SetActive(true);
        shopButtons["Utility"].SetActive(true);
    }
}
