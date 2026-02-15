using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bossHealthBarPanel; // Panel holding the health bar UI
    [SerializeField] private Image healthBar;               // The fill image for the health bar
    [SerializeField] private Health health;                 // Reference to Boss's Health

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnDamaged.AddListener(UpdateHealthUI);
            health.OnDeath.AddListener(HandleDeath);

            // Sync immediately on enable
            UpdateHealthUI(health.GetCurrentHealth());
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDamaged.RemoveListener(UpdateHealthUI);
            health.OnDeath.RemoveListener(HandleDeath);
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        if (healthBar != null && health != null)
        {
            float normalized = (float)currentHealth / health.GetMaxHealth();
            healthBar.fillAmount = normalized;
        }
    }

    public void HandleDeath()
    {
        // Hide the boss health bar panel when boss dies
        if (bossHealthBarPanel != null) 
            bossHealthBarPanel.SetActive(false);
    }
}