using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TowerBuyEvent : UnityEvent<GameObject> { }

public class BuyTower : MonoBehaviour
{
    public TowerCardManager TowerCardManager;
    public TowerBuyEvent EvtOnBuySuccessful;
    
    private TowerNodeManager CurrentTowerNode;
    private int cost;

    private void OnEnable()
    {
        CurrentTowerNode = GameManager.Instance.CurrentTowerNode;
        cost = TowerCardManager.GetCostValue();
    }

    // === Generic gold spending wrapper ===
    private bool TrySpendGold(int amount)
    {
        if (GameManager.Instance.GoldManager?.SpendGold(amount) == true)
            return true;

        Debug.Log("Not enough gold.");
        return false;
    }

    public void _BuyButtonClicked()
    {
        if (TrySpendGold(cost))
        {
            DespawnCurrentTower();
            EvtOnBuySuccessful?.Invoke(TowerCardManager.TowerPrefab);
        }
    }

    public void _DespawnButtonClicked()
    {
        if (TrySpendGold(cost))
        {
            DespawnCurrentTower();
            GameManager.Instance.GoldManager?.AddGold(cost); // refund
        }
    }

    public void _RepairButtonClicked()
    {
        if (TrySpendGold(cost))
        {
            CurrentTowerNode?.towerController?.TowerHealth
                ?.Heal(CurrentTowerNode.towerController.TowerHealth.GetMaxHealth());
        }
    }

    public void _IncreaseDamageButtonClicked()
    {
        if (TrySpendGold(cost))
        {
            CurrentTowerNode?.towerController?.IncreaseTowerMainDamage();
            Debug.Log("Tower Damage Level Increased");
        }
    }

    // === Shared despawn logic ===
    public void DespawnCurrentTower()
    {
        var node = GameManager.Instance.CurrentTowerNode;

        if (node?.towerController != null)
        {
            GameManager.Instance.DespawnTower(node.towerController);
            node.towerController = null;
        }
        else
        {
            Debug.Log("No tower to despawn.");
        }
    }
}
