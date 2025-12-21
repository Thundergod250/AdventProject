using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Global references
    public PlayerController PlayerController;
    public CameraManager CameraManager;
    public UI_Manager UIManager;
    public GoldManager GoldManager;
    public ObjectPooling ObjectPooling;
    public DebugCheats DebugCheats;

    [HideInInspector] public TowerNodeManager CurrentTowerNode;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject SpawnObject(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogWarning("No prefab provided to SpawnObject.");
            return null;
        }

        GameObject obj = ObjectPooling.Instance.Get(prefab, parent);

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.localScale = prefab.transform.localScale;

        return obj;
    }

    public void SpawnTower(GameObject towerPrefab)
    {
        if (CurrentTowerNode == null || CurrentTowerNode.spawnTransform == null)
        {
            Debug.LogWarning("No tower node or spawn transform assigned.");
            return;
        }

        if (towerPrefab == null)
        {
            Debug.LogWarning("No tower prefab provided.");
            return;
        }

        // Despawn existing tower if present
        if (CurrentTowerNode.towerController != null)
        {
            DespawnTower(CurrentTowerNode.towerController);
        }

        // Spawn new tower
        GameObject towerGO = SpawnObject(
            towerPrefab,
            CurrentTowerNode.transform,
            CurrentTowerNode.spawnTransform.position,
            towerPrefab.transform.rotation
        );

        TowerController controller = towerGO.GetComponent<TowerController>();
        controller.towerPrefab = towerPrefab;

        // Track controller on the node
        CurrentTowerNode.towerController = controller;

        Debug.Log($"Spawned tower under {CurrentTowerNode.name}");

        CurrentTowerNode = null;
    }

    public void DespawnTower(TowerController controller)
    {
        if (controller == null || controller.towerPrefab == null) return;

        ObjectPooling.Instance.Return(controller.towerPrefab, controller.towerInstance);
        Debug.Log($"Tower {controller.name} returned to pool.");
    }
}
