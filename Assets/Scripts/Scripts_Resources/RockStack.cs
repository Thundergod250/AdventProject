using UnityEngine;

public class RockStack : MonoBehaviour
{
    public int RockStackLevel = 0;
    public GameObject[] rockPrefabs; // Assign different upgrade rocks in inspector
    public int baseCost = 50;
    public int costIncreasePerLevel = 25;

    private GameObject currentRock;

    public int GetCurrentCost()
    {
        return baseCost + (RockStackLevel * costIncreasePerLevel);
    }

    public bool TryUpgrade(GoldManager goldManager)
    {
        int cost = GetCurrentCost();
        if (goldManager.SpendGold(cost))
        {
            ApplyUpgrade();
            return true;
        }
        return false;
    }

    private void ApplyUpgrade()
    {
        // Disable current rock if exists
        if (currentRock != null)
            currentRock.SetActive(false);

        // Increase level
        RockStackLevel++;

        // Enable new rock if available
        if (RockStackLevel - 1 < rockPrefabs.Length)
        {
            currentRock = rockPrefabs[RockStackLevel - 1];
            currentRock.SetActive(true);
        }
    }
}