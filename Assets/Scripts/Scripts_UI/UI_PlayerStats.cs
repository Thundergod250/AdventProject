using TMPro;
using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI playerDamage;
    public TextMeshProUGUI playerAttackRadius;

    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        if (playerStats != null)
        {
            // Subscribe to stat change event
            playerStats.EvtOnStatChange.AddListener(UpdateUI);

            // Initialize UI
            UpdateUI();
        }
        else
            Debug.LogWarning($"{name}: PlayerStats not found in scene!");
    }

    private void UpdateUI()
    {
        if (playerStats == null) return;

        if (playerDamage != null)
            playerDamage.text = $"Damage: {playerStats.damage}";

        if (playerAttackRadius != null)
            playerAttackRadius.text = $"Attack Radius: {playerStats.attackRange}";
    }
}