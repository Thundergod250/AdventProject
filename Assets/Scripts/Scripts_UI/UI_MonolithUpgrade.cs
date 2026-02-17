using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_MonolithUpgrade : MonoBehaviour
{
    [Header("Damage & Range Text")]
    [SerializeField] private TextMeshProUGUI damageText;   // assign in Inspector
    [SerializeField] private TextMeshProUGUI rangeText;    // assign in Inspector

    [Header("Upgrade Button")]
    [SerializeField] private TextMeshProUGUI upgradeButtonText; // assign in Inspector
    [SerializeField] private Button addStatsButton;             // assign in Inspector

    [Header("Feedback Text")]
    [SerializeField] private TextMeshProUGUI notEnoughGoldText; // assign in Inspector

    public void ToggleMonolithUpgradeUI()
    {
        GameManager.Instance.UIManager.ToggleUI(UIPanelType.Monolith);
    }

    // Call this to update the displayed values
    public void SetStats(int damage, int range)
    {
        if (damageText != null)
            damageText.text = $"+{damage}";

        if (rangeText != null)
            rangeText.text = $"+{range}";
    }

    // Call this to update the button text with the current price
    public void SetUpgradePrice(int price)
    {
        if (upgradeButtonText != null)
            upgradeButtonText.text = $"Cost: ({price} Gold)";
    }

    // Coroutine to show "Not enough Gold" feedback
    public void ShowNotEnoughGold()
    {
        if (notEnoughGoldText != null)
            StartCoroutine(NotEnoughGoldRoutine());
    }

    private IEnumerator NotEnoughGoldRoutine()
    {
        // Disable the Add Stats button
        if (addStatsButton != null)
            addStatsButton.interactable = false;

        // Show feedback text
        notEnoughGoldText.gameObject.SetActive(true);

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Hide feedback text
        notEnoughGoldText.gameObject.SetActive(false);

        // Re-enable the Add Stats button
        if (addStatsButton != null)
            addStatsButton.interactable = true;
    }
}
