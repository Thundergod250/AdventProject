using UnityEngine;
using UnityEngine.Events;

public class MoneyManipulator : MonoBehaviour
{
    public UnityEvent HasEnoughMoney;
    public UnityEvent DoesNotHaveEnoughMoney;

    public void _AddMoney(int amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.GoldManager != null)
        {
            GameManager.Instance.GoldManager.AddGold(amount);
            Debug.Log($"Added {amount} gold. Current gold: {GameManager.Instance.GoldManager.PlayerGold}");
        }
    }
    
    public void _SpendMoney(int amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.GoldManager != null)
        {
            bool success = GameManager.Instance.GoldManager.SpendGold(amount);
            if (success)
            {
                HasEnoughMoney?.Invoke();
                Debug.Log($"Spent {amount} gold. Current gold: {GameManager.Instance.GoldManager.PlayerGold}");
            }
            else
            {
                DoesNotHaveEnoughMoney?.Invoke();
                Debug.LogWarning($"Not enough gold to spend {amount}. Current gold: {GameManager.Instance.GoldManager.PlayerGold}");
            }
        }
    }

    public void _ReduceMoney(int amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.GoldManager != null)
        {
            GameManager.Instance.GoldManager.ReduceGold(amount);
            Debug.Log($"Reduced {amount} gold (forced). Current gold: {GameManager.Instance.GoldManager.PlayerGold}");
        }
    }

    public int _GetGold()
    {
        return GameManager.Instance.GoldManager.GetGold(); 
    }
}