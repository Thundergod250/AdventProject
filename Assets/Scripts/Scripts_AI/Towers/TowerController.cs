using UnityEngine;
using UnityEngine.Events;

public class TowerController : MonoBehaviour
{
    [HideInInspector] public GameObject towerPrefab;   // prefab used to spawn this tower
    [HideInInspector] public GameObject towerInstance; // actual GameObject (this one)
    
    public UnityEvent EvtOnIncreaseTowerMainDamage;
    public UnityEvent EvtOnIncreaseTowerAtkRate; 

    [SerializeField] private TowerCategoryData_SO thisTowerUpgradeCards_SO; 
    [SerializeField] private Transform attackLocation; 
    public Health TowerHealth;
    
    private void Awake() => towerInstance = gameObject;

    public void IncreaseTowerMainDamage() => EvtOnIncreaseTowerMainDamage?.Invoke();

    public void DespawnCurrentTower()
    {
        // Search parent for TowerNodeManager
        TowerNodeManager nodeManager = GetComponentInParent<TowerNodeManager>();
        if (nodeManager != null)
            nodeManager.DespawnTower();
        else
            Debug.LogWarning($"{name} could not find TowerNodeManager in parent hierarchy.");
    }

    public TowerCategoryData_SO GetUpgradeData() => thisTowerUpgradeCards_SO;

    public Transform GetAttackLocation() => attackLocation;
}