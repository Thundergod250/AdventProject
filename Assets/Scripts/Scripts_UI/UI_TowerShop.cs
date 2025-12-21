using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_TowerShop : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform cardParent;
    [SerializeField] private TowerCategoryData_SO offensiveTowersData;
    [SerializeField] private TowerCategoryData_SO defensiveTowersData;
    [SerializeField] private TowerCategoryData_SO utilityTowersData;

    [Header("Shop Buttons")]
    [SerializeField] private GameObject towerUpgradesButton;
    [SerializeField] private GameObject offensiveButton;
    [SerializeField] private GameObject defensiveButton;
    [SerializeField] private GameObject utilityButton;

    private TowerCategoryData_SO towerUpgradesData;
    private Dictionary<string, GameObject> shopButtons;
    private readonly List<GameObject> activeCards = new();
    private TowerCategoryData_SO currentCategory;

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

    public void SetUpgradeCategoryData(TowerCategoryData_SO data) => towerUpgradesData = data;

    // === Category entry points ===
    public void OpenTowerUpgrades() => TrySpawnCategory(towerUpgradesData);
    public void OpenOffensiveTowers() => TrySpawnCategory(offensiveTowersData);
    public void OpenDefensiveTowers() => TrySpawnCategory(defensiveTowersData);
    public void OpenUtilityTowers() => TrySpawnCategory(utilityTowersData);

    // === Category spawning ===
    private void TrySpawnCategory(TowerCategoryData_SO data)
    {
        if (data == null) return;
        if (currentCategory == data) return;

        ClearCards();
        currentCategory = data;
        SpawnCards(data.cards);
    }


    // === Card spawning ===
    private void SpawnCards(List<CardInfo> cards)
    {
        foreach (var cardInfo in cards)
        {
            if (cardInfo.towerCardPrefab == null)
            {
                Debug.LogError($"Card prefab missing for {cardInfo.title}");
                continue;
            }

            GameObject cardGO = ObjectPooling.Instance.Get(cardInfo.towerCardPrefab, cardParent);
            cardGO.SetActive(true);

            TowerCardManager card = cardGO.GetComponent<TowerCardManager>();
            if (card != null)
            {
                card.ResetCard(cardInfo);
                card.SetSourcePrefab(cardInfo.towerCardPrefab);
            }

            BuyTower buyTower = cardGO.GetComponent<BuyTower>();
            if (buyTower != null)
                buyTower.TowerCardManager = card;

            activeCards.Add(cardGO);
        }
    }

    // === Clear old cards ===
    public void ClearCards()
    {
        foreach (var card in activeCards)
        {
            TowerCardManager manager = card.GetComponent<TowerCardManager>();
            if (manager != null && manager.GetSourcePrefab() != null)
                ObjectPooling.Instance.Return(manager.GetSourcePrefab(), card);
            else
                Destroy(card);
        }
        activeCards.Clear();
        currentCategory = null;
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
