using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouristSpawner : MonoBehaviour
{
    [Header("Tourist Setup")]
    public List<GameObject> Tourist_Attractions = new List<GameObject>(); // drop attraction GameObjects here
    [SerializeField] private GameObject food_Stall;
    [SerializeField] private GameObject return_To_Ship;

    [SerializeField] private GameObject tourist_Prefab;
    [SerializeField] private int number_Of_Tourists = 5;
    [SerializeField] private float spawn_Delay = 1f;

    private void Awake()
    {
        StartCoroutine(SpawnTourists());
    }

    private IEnumerator SpawnTourists()
    {
        for (int i = 0; i < number_Of_Tourists; i++)
        {
            Vector3 offset = new Vector3(i * 1.5f, 0f, 0f);
            GameObject tourist = Instantiate(tourist_Prefab, transform.position + offset, Quaternion.identity);

            //GameManager.Instance.Tourist_Manager_Ref.tourists.Add(tourist.GetComponent<TouristMovement>());
            TouristMovement movement = tourist.GetComponent<TouristMovement>();

            movement.SetReturnToShip(return_To_Ship);
            // Give the food stall reference
            movement.SetFoodStall(food_Stall);

            // Assign a random attraction if list is not empty
            if (Tourist_Attractions.Count > 0)
            {
                GameObject randomAttraction = Tourist_Attractions[Random.Range(0, Tourist_Attractions.Count)];
                movement.SetDestination(randomAttraction);
            }

            yield return new WaitForSeconds(spawn_Delay);
        }
    }
}
