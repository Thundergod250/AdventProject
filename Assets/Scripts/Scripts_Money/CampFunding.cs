using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CampFunding : MonoBehaviour
{
    [Header("Funding Settings")]
    public int money;                // current stored money
    public int moneyPerSecond = 1;   // how much is generated per second

    [Header("Events")]
    public UnityEvent<int> EvtOnMoneyChange; // passes current money to UI

    private Coroutine fundingRoutine;

    private void Start() => fundingRoutine = StartCoroutine(GenerateMoneyRoutine());

    private IEnumerator GenerateMoneyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            money += moneyPerSecond;

            // Notify UI
            EvtOnMoneyChange?.Invoke(money);
        }
    }
    
    public int CollectMoney()
    {
        int collected = money;
        GameManager.Instance.GoldManager.AddGold(collected);

        // Reset storage
        money = 0;

        // Notify UI
        EvtOnMoneyChange?.Invoke(money);

        return collected;
    }
    
    public void StopFunding()
    {
        if (fundingRoutine == null) return;
        
        StopCoroutine(fundingRoutine);
        fundingRoutine = null;
    }
}