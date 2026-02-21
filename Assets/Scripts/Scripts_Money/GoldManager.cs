using UnityEngine;
using UnityEngine.Events;

public class GoldManager : MonoBehaviour
{
    [Header("Gold Settings")]
    [SerializeField] private int playerGold = 100;
    public int PlayerGold => playerGold;

    [Header("Events")]
    public UnityEvent<int> EvtOnGoldChanged; // passes new gold value

    public void AddGold(int amount)
    {
        playerGold += Mathf.Max(0, amount);
        EvtOnGoldChanged?.Invoke(playerGold);
    }

    public bool SpendGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            playerGold -= amount;
            EvtOnGoldChanged?.Invoke(playerGold);
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

    public void DivideGold(int amount)
    {
        playerGold = Mathf.Max(0, playerGold / amount);
        EvtOnGoldChanged?.Invoke(playerGold);
    }
}