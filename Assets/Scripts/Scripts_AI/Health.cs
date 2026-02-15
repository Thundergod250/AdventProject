using UnityEngine;
using UnityEngine.Events;

public enum Faction
{
    Player,
    Enemy,
    Ally,
    Neutral
}

public enum DamageMode
{
    Normal,
    Invincible,
    Fortified
}

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Faction Settings")]
    [SerializeField] private Faction faction;
    public Faction GetFaction() => faction;

    [Header("Damage Mode Settings")]
    [SerializeField] private DamageMode damageMode = DamageMode.Normal;
    public DamageMode GetDamageMode() => damageMode;
    public void SetDamageMode(DamageMode mode) => damageMode = mode;

    [Header("Events")]
    public UnityEvent<int> OnDamaged;   // passes remaining health
    public UnityEvent OnDeath;          // triggered when health <= 0

    private bool isDead = false;

    [SerializeField] private int startSetHealth; //[FOR TESTING] 

    private void Awake()
    {
        currentHealth = maxHealth;
        currentHealth = startSetHealth;
        
        OnDamaged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        switch (damageMode)
        {
            case DamageMode.Invincible:
                // No damage taken
                return;

            case DamageMode.Fortified:
                // All damage reduced to 1
                amount = 1;
                break;

            case DamageMode.Normal:
                // Use amount as-is
                break;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        // Trigger damage event
        OnDamaged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();

        Debug.Log($"{gameObject.name} has died.");
        // Optional: Destroy(gameObject); or disable
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
