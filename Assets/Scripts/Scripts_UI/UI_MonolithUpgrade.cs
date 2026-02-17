using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonolithUpgrade : MonoBehaviour
{
    [Header("Damage & Range Text")]
    public TextMeshProUGUI damageText;   // assign in Inspector
    public TextMeshProUGUI rangeText;    // assign in Inspector
    
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
}
