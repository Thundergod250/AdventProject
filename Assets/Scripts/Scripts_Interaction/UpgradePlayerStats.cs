using UnityEngine;

public class UpgradePlayerStats : MonoBehaviour
{
    // Base stats
    public float damage = 10;
    public int radius = 2; 
       
    // Function to upgrade player stats
    public void UpgradeStats() 
    {
        GameManager.Instance.PlayerController.PlayerAttack.AddAttackValues(10,2);
    }
}
