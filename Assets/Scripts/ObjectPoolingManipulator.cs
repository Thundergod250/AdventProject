using UnityEngine;

public class ObjectPoolingManipulator : MonoBehaviour
{
    public GameObject _Spawn(GameObject prefab, Transform parent = null) => ObjectPooling.Instance?.Get(prefab, parent);

    public void _Return(GameObject prefab, GameObject instance) => ObjectPooling.Instance?.Return(prefab, instance);

    public void _ReturnSelf(GameObject prefabRef) => ObjectPooling.Instance?.Return(prefabRef, gameObject);
}