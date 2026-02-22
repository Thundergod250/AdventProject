using UnityEngine;
using UnityEngine.Events;

public class GoldManager : MonoBehaviour
{
    [Header("Gold Settings")]
    [SerializeField] private int playerGold = 100;
    public int PlayerGold => playerGold;

    [Header("Events")]
    public UnityEvent<int> EvtOnGoldChanged;

    public void AddGold(int amount)
    {
        playerGold += Mathf.Max(0, amount);
        EvtOnGoldChanged?.Invoke(playerGold);
    }
    
    public bool SpendGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            ReduceGold(amount);
            return true;
        }
        return false;
    }

    public int GetGold() => PlayerGold;

    public void ResetGold()
    {
        playerGold = 0;
        EvtOnGoldChanged?.Invoke(playerGold);
    }

    public bool HasEnoughGold(int amount) => playerGold >= amount;
    
    public void ReduceGold(int amount)
    {
        playerGold = Mathf.Max(0, playerGold - amount);
        EvtOnGoldChanged?.Invoke(playerGold);
    }
    
    public bool SpendGoldPercentage(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        int amount = Mathf.RoundToInt(playerGold * percentage);

        if (HasEnoughGold(amount))
        {
            ReduceGold(amount);
            return true;
        }
        return false;
    }
    
    public void ReduceGoldPercentage(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        int amount = Mathf.RoundToInt(playerGold * percentage);

        playerGold = Mathf.Max(0, playerGold - amount);
        EvtOnGoldChanged?.Invoke(playerGold);
    }
}
