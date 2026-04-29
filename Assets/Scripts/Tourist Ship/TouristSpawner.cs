using UnityEngine;

public class TouristSpawner : MonoBehaviour
{
    [SerializeField] private GameObject touristPrefab; // assign your Tourist prefab here
    [SerializeField] private Transform spawnPoint;     // where tourists will spawn
    [SerializeField] private int numberOfTourists = 5; // how many to spawn

    public void SpawnTourists()
    {
        for (int i = 0; i < numberOfTourists; i++)
        {
            // Optionally offset each tourist slightly so they don’t overlap
            Vector3 offset = new Vector3(i * 1.5f, 0f, 0f);
            Instantiate(touristPrefab, spawnPoint.localPosition + offset, Quaternion.identity);
        }
    }
}
