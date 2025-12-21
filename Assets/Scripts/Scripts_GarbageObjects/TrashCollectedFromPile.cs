using UnityEngine;
using UnityEngine.Events;

public class TrashCollectedFromPile : MonoBehaviour
{
    public UnityEvent<GameObject> EvtOnTrashPileInteract;

    [Header("Trash Prefabs")]
    [SerializeField] private GameObject[] trashPrefabs;

    [Header("Settings")]
    [SerializeField] private int totalTrashCount = 3;

    public void _InteractTrashPile()
    {
        if (totalTrashCount <= 0 || trashPrefabs.Length == 0) return;

        // Pick random prefab
        GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];

        // Spawn via ObjectPooling
        GameObject newTrash = GameManager.Instance.SpawnObject(prefab, null, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        // Notify listeners
        EvtOnTrashPileInteract?.Invoke(newTrash);

        // Reduce count
        totalTrashCount--;

        // Return pile to pool if empty
        if (totalTrashCount <= 0)
            GameManager.Instance.ObjectPooling.Return(gameObject, gameObject);
    }
}