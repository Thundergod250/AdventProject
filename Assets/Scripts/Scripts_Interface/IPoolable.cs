using UnityEngine;

public interface IPoolable
{
    void SetSourcePrefab(GameObject prefab);
    GameObject GetSourcePrefab();
}