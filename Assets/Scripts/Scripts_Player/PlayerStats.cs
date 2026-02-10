using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 25;
    public int attackRange = 3;

    [Header("Events")]
    public UnityEvent EvtOnStatChange; 
    
    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        ApplyStats(); // initialize PlayerAttack with current stats
        TriggerStatChange(); // update UI at start
    }

    // Call this whenever stats are upgraded
    public void UpgradeStats(int newDamage, int newRange)
    {
        damage = newDamage;
        attackRange = newRange;
        ApplyStats();
        TriggerStatChange();
    }

    private void ApplyStats()
    {
        if (playerAttack != null) 
            playerAttack.SetAttackValues(damage, attackRange);
    }

    private void TriggerStatChange()
    {
        EvtOnStatChange?.Invoke();
    }
}