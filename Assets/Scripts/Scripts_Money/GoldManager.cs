using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public int playerGold = 100;
    public int PlayerGold => playerGold; // read-only property
    
    public void AddGold(int amount) => playerGold += Mathf.Max(0, amount);

    public bool SpendGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            playerGold -= amount;
            return true;
        }
        return false;
    }

    public int GetGold()
    {
        return PlayerGold;
    }

    public void ResetGold() => playerGold = 0;
    public bool HasEnoughGold(int amount) => playerGold >= amount;
    public void ReduceGold(int amount) => playerGold = Mathf.Max(0, playerGold - amount);
    public void DivideGold(int amount) => playerGold = Mathf.Max(0, playerGold / amount);

}