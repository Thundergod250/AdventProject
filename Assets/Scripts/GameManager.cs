using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Global references
    public PlayerController PlayerController;
    public PlayerDeathManager PlayerDeathManager;
    public CameraManager CameraManager;
    public UI_Manager UIManager;
    public GoldManager GoldManager;
    public ObjectPooling ObjectPooling;
    public DebugCheats DebugCheats;
    public PlayerInventory PlayerInventory;
    public UI_ReticleRaycast UI_ReticleRaycast;
    public FreeLookCamControl FreeLookCamControl;
    public MiningManager MiningManager;

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
}
