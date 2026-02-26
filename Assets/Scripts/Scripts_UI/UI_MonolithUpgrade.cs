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
    [SerializeField] private TextMeshProUGUI costButtonText; // assign in Inspector
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
        if (costButtonText != null)
            costButtonText.text = $"Cost:{price}";
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

        // Change button text to "Not Enough Gems" and set color to red
        if (notEnoughGoldText != null)
        {
            notEnoughGoldText.text = "Not Enough Gems";
            notEnoughGoldText.color = Color.red;
        }

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Revert button text back to "Buy Upgrade" and set color to black
        if (notEnoughGoldText != null)
        {
            notEnoughGoldText.text = "Buy Upgrade";
            notEnoughGoldText.color = Color.black;
        }

        // Re-enable the Add Stats button
        if (addStatsButton != null)
            addStatsButton.interactable = true;
    }
}
