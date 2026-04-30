using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouristSpawner : MonoBehaviour
{
    [Header("Tourist Setup")]
    public List<GameObject> TouristAttractions = new List<GameObject>(); // drop attraction GameObjects here
    [SerializeField] private GameObject foodStall;

    [SerializeField] private GameObject touristPrefab;
    [SerializeField] private int numberOfTourists = 5;
    [SerializeField] private float spawnDelay = 1f;

    private void Awake()
    {
        StartCoroutine(SpawnTourists());
    }

    private IEnumerator SpawnTourists()
    {
        for (int i = 0; i < numberOfTourists; i++)
        {
            Vector3 offset = new Vector3(i * 1.5f, 0f, 0f);
            GameObject tourist = Instantiate(touristPrefab, transform.position + offset, Quaternion.identity);

            TouristMovement movement = tourist.GetComponent<TouristMovement>();

            // Give the food stall reference
            movement.SetFoodStall(foodStall);

            // Assign a random attraction if list is not empty
            if (TouristAttractions.Count > 0)
            {
                GameObject randomAttraction = TouristAttractions[Random.Range(0, TouristAttractions.Count)];
                movement.SetDestination(randomAttraction);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
