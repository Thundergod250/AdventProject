using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bossHealthBarPanel; // Panel holding the health bar UI
    [SerializeField] private Image healthBar;               // The fill image for the health bar
    private Health health;                 // Reference to Boss's Health
    
    /*private void OnEnable()
    {
        if (health != null)
        {
            health.OnDamaged.AddListener(UpdateHealthUI);
            health.OnDeath.AddListener(HandleDeath);

            // Sync immediately on enable
            UpdateHealthUI(health.GetCurrentHealth());
        }
    }*/

    private void OnDisable()
    {
        bossHealthBarPanel.SetActive(false);
        
        if (health != null)
        {
            health.OnDamaged.RemoveListener(UpdateHealthUI);
            health.OnDeath.RemoveListener(HandleDeath);
        }
    }

    public void OnActivate(Health hp)
    {
        Debug.Log("Boss Health Bar Activated");
        health = hp; 
        bossHealthBarPanel.SetActive(true);
            
        health.OnDamaged.AddListener(UpdateHealthUI);
        health.OnDeath.AddListener(HandleDeath);

        // Sync immediately on enable
        UpdateHealthUI(health.GetCurrentHealth());
    }

    public void OnDeactivate()
    {
        Debug.Log("Boss Health Bar Deactivated");
        bossHealthBarPanel.SetActive(false);
        
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

    public void HandleDeath() => OnDeactivate();
}