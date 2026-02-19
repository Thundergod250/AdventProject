using UnityEngine;

public class Monolith : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // assign in Inspector

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }
}
