using UnityEngine;

public class TowerNodeManager : MonoBehaviour
{
    public TowerController towerController;   
    public Transform spawnTransform;

    public void DespawnTower()
    {
        if (towerController != null)
        {
            GameManager.Instance.DespawnTower(towerController);
            towerController = null;
        }
        
        else
            Debug.Log("Tower Controller not found");
    }
}