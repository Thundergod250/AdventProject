using UnityEngine;
using UnityEngine.Events;

public class MoneyManipulator : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent HasEnoughMoney;
    public UnityEvent DoesNotHaveEnoughMoney;

    private GoldManager Gold => GameManager.Instance?.GoldManager;

    public void _AddMoney(int amount)
    {
        if (Gold != null)
        {
            Gold.AddGold(amount);
            Debug.Log($"Added {amount} gold. Current gold: {Gold.PlayerGold}");
        }
        else
            Debug.LogError("GoldManager not found in GameManager.");
    }

    public void _SpendMoney(int amount)
    {
        if (Gold != null)
        {
            bool success = Gold.SpendGold(amount);
            if (success)
            {
                HasEnoughMoney?.Invoke();
                Debug.Log($"Spent {amount} gold. Current gold: {Gold.PlayerGold}");
            }
            else
            {
                DoesNotHaveEnoughMoney?.Invoke();
                Debug.LogWarning($"Not enough gold to spend {amount}. Current gold: {Gold.PlayerGold}");
            }
        }
        else
            Debug.LogError("GoldManager not found in GameManager.");
    }

    public void _ReduceMoney(int amount)
    {
        if (Gold != null)
        {
            Gold.ReduceGold(amount);
            Debug.Log($"Reduced {amount} gold (forced). Current gold: {Gold.PlayerGold}");
        }
        else
            Debug.LogError("GoldManager not found in GameManager.");
    }

    public void _SpendMoneyPercentage(float percentage)
    {
        if (Gold != null)
        {
            bool success = Gold.SpendGoldPercentage(percentage);
            if (success)
            {
                HasEnoughMoney?.Invoke();
                Debug.Log($"Spent {percentage * 100f}% of gold. Current gold: {Gold.PlayerGold}");
            }
            else
            {
                DoesNotHaveEnoughMoney?.Invoke();
                Debug.LogWarning($"Not enough gold to spend {percentage * 100f}%. Current gold: {Gold.PlayerGold}");
            }
        }
        else
            Debug.LogError("GoldManager not found in GameManager.");
    }

    public void _ReduceMoneyPercentage(float percentage)
    {
        if (Gold != null)
        {
            Gold.ReduceGoldPercentage(percentage);
            Debug.Log($"Reduced {percentage * 100f}% of gold (forced). Current gold: {Gold.PlayerGold}");
        }
        else
            Debug.LogError("GoldManager not found in GameManager.");
    }

    public void _ResetMoney()
    {
        if (Gold != null)
        {
            Gold.ResetGold();
            Debug.Log("Gold reset to 0.");
        }
        else
            Debug.LogError("GoldManager not found in GameManager.");
    }

    public int _GetGold()
    {
        if (Gold != null)
            return Gold.GetGold();
        Debug.LogError("GoldManager not found in GameManager.");
        return 0;
    }
}
