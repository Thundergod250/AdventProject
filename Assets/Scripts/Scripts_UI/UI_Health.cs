using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Health healthSource; // 👈 reference to the Health component

    private void Awake()
    {
        if (healthSource != null)
        {
            // Subscribe to health events
            healthSource.OnDamaged.AddListener(UpdateHealthBar);
            healthSource.OnDeath.AddListener(HideHealthBar);
        }

        // Initialize bar
        UpdateHealthBar(healthSource.GetCurrentHealth());
    }

    private void UpdateHealthBar(int currentHealth)
    {
        if (healthBar == null || healthSource == null) return;

        float fillAmount = (float)currentHealth / healthSource.GetMaxHealth();
        healthBar.fillAmount = fillAmount;
    }

    private void HideHealthBar()
    {
        if (healthBar != null)
            healthBar.gameObject.SetActive(false);
    }
}