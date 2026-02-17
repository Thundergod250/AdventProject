using UnityEngine;

public class UpgradePlayerStats : MonoBehaviour
{
    // Base stats
    public int damage = 5;
    public int radius = 2;

    [Header("Upgrade Settings")]
    [SerializeField] private int upgradePrice = 50;          // starting price
    [SerializeField] private int upgradePriceIncrease = 25;  // how much the price increases each upgrade

    [SerializeField] private UI_MonolithUpgrade ui_MonolithUpgrade;

    public void SetUI()
    {
        ui_MonolithUpgrade.ToggleMonolithUpgradeUI();
        ui_MonolithUpgrade.SetStats(damage, radius); // show initial values
    }

    // Function to upgrade player stats
    public void UpgradeStats()
    {
        // Upgrade player attack values
        GameManager.Instance.PlayerController.PlayerAttack.AddAttackValues(damage, radius);

        // Update UI
        ui_MonolithUpgrade.SetStats(damage, radius);
    }

    // Function for the Upgrade button in the UI
    public void UpgradeButton()
    {
        if (GameManager.Instance.GoldManager.playerGold >= upgradePrice)
        {
            // Deduct gold
            GameManager.Instance.GoldManager.playerGold -= upgradePrice;

            // Increase stats
            damage += 5;
            radius += 1;

            // Increase upgrade price
            upgradePrice += upgradePriceIncrease;

            // Apply new stats to player attack

            // Update UI
            ui_MonolithUpgrade.SetStats(damage, radius);
        }
        else
        {
            Debug.Log("Not enough gold to upgrade!");
        }
    }
}
