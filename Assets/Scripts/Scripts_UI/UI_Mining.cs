using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Mining : MonoBehaviour
{
    [Header("Upgrade UI")]
    public TextMeshProUGUI UiMineDurationPriceText;
    public GameObject UiMineDurationLabelText;
    public TextMeshProUGUI UiMineQualityPriceText;
    public GameObject UiMineQualityLabelText;

    // References to the actual buttons
    public Button MineDurationButton;
    public Button MineQualityButton;

    [Header("Level Text References")]
    public TextMeshProUGUI RockLevelText;
    public TextMeshProUGUI EnemyLevelText;

    public void ToggleMiningUI()
    {
        GameManager.Instance.UIManager.ToggleUI(UIPanelType.Mine);
    }

    // Update methods for MiningManager to call
    public void UpdateRockLevel(int rockIndex)
    {
        RockLevelText.text = $"Rock Level: {rockIndex + 1}";
    }

    public void UpdateEnemyLevel(int enemyIndex)
    {
        EnemyLevelText.text = $"Enemy Level: {enemyIndex + 1}";
    }

    public void UpdateMineDurationPrice(int price)
    {
        UiMineDurationPriceText.text = $"Upgrade Mine Duration Price: {price}";
    }

    public void UpdateMineQualityPrice(int price)
    {
        UiMineQualityPriceText.text = $"Upgrade Rock & Enemy Level Price: {price}";
    }
}
