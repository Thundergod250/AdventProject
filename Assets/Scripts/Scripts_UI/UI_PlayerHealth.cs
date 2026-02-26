using UnityEngine;

public class UI_PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] healthGems;   // Assign gems in order
    [SerializeField] private Health health;             // Reference to Player's Health

    private void OnEnable()
    {
        // Subscribe to Health events
        if (health != null)
        {
            health.OnDamaged.AddListener(UpdateHealthUI);
            health.OnDeath.AddListener(HandleDeath);

            // Sync immediately
            UpdateHealthUI(health.GetCurrentHealth());
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid leaks
        if (health != null)
        {
            health.OnDamaged.RemoveListener(UpdateHealthUI);
            health.OnDeath.RemoveListener(HandleDeath);
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < healthGems.Length; i++)
            healthGems[i].SetActive(i < currentHealth);
    }

    public void HandleDeath()
    {
        foreach (var gem in healthGems)
            gem.SetActive(false);
    }
}
