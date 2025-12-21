using System;
using UnityEngine;
using UnityEngine.Events;

public class TowerNodeManagerManipulator : MonoBehaviour
{
    public UnityEvent<TowerController> EvtOnInteractWithTowerController;
    public UnityEvent<TowerCategoryData_SO> EvtOnInteractWithTowerControllerPassSO;
    
    private TowerNodeManager towerNodeManager;

    private void OnEnable()
    {
        towerNodeManager = GetComponent<TowerNodeManager>();
    }

    public void _SendTowerController()
    {
        EvtOnInteractWithTowerController?.Invoke(towerNodeManager?.towerController);
        EvtOnInteractWithTowerControllerPassSO?.Invoke(towerNodeManager?.towerController?.GetUpgradeData());
    }
    
    public void _PassTowerCategoryS0(TowerCategoryData_SO data) => GameManager.Instance.UIManager.UI_TowerShop?.SetUpgradeCategoryData(data);
}
