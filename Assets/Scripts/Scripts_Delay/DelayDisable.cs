using UnityEngine;

public class DelayDisable : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;   // editable in Inspector
    private float timer;

    // Reference to the prefab this instance belongs to (set by spawner)
    private GameObject prefabRef;

    private void OnEnable() => timer = lifetime;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) 
            ReturnToPool();
    }
    
    public void SetPrefabReference(GameObject prefab) => prefabRef = prefab;

    private void ReturnToPool()
    {
        if (GameManager.Instance != null && GameManager.Instance.ObjectPooling != null && prefabRef != null)
            GameManager.Instance.ObjectPooling.Return(prefabRef, gameObject);
        else
            gameObject.SetActive(false);
    }
}