using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Events")]
    public UnityEvent<int> OnDamaged;   // passes remaining health
    public UnityEvent OnDeath;          // triggered when health <= 0
    
    private bool isDead = false;

    [SerializeField] private int startSetHealth; //[FOR TESTING] 

    private void Awake()
    {
        currentHealth = maxHealth;
        currentHealth = startSetHealth;
    }
    
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        // Trigger damage event
        OnDamaged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Die()
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