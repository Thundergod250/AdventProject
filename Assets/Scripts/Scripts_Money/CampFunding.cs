using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CampFunding : MonoBehaviour
{
    [Header("Funding Settings")]
    public int money;                     // current stored money
    public int moneyPerSecond = 1;        // how much is generated per second
    [SerializeField] private int updateInterval = 5;

    [Header("Events")]
    public UnityEvent<int> EvtOnMoneyChange;       // passes committed money to UI
    public UnityEvent<int> EvtOnRateChange;        // passes profit per second to UI

    private Coroutine fundingRoutine;
    private int accumulated;
    private int elapsedSeconds;

    private void Start()
    {
        fundingRoutine = StartCoroutine(GenerateMoneyRoutine());
        EvtOnRateChange?.Invoke(moneyPerSecond);
    }

    private IEnumerator GenerateMoneyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            accumulated += moneyPerSecond;
            elapsedSeconds++;

            if (elapsedSeconds >= updateInterval)
            {
                money += accumulated;
                accumulated = 0;
                elapsedSeconds = 0;

                EvtOnMoneyChange?.Invoke(money);
            }
        }
    }

    public int CollectMoney()
    {
        int collected = money;
        GameManager.Instance.GoldManager.AddGold(collected);

        money = 0;
        EvtOnMoneyChange?.Invoke(money);

        return collected;
    }

    public void StopFunding()
    {
        if (fundingRoutine == null) return;
        StopCoroutine(fundingRoutine);
        fundingRoutine = null;
    }
    
    public void SetMoneyPerSecond(int newRate)
    {
        moneyPerSecond = newRate;
        EvtOnRateChange?.Invoke(moneyPerSecond);
    }
}