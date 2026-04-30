using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TouristShipMovement Tourist_Ship_Movement_Ref;
    public TouristSpawner Tourist_Spawner_Ref;
    public StallRating Stall_Rating_Ref;
    //public TouristManager Tourist_Manager_Ref;

    private void Awake()
    {
        // If there's an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // Optional: keep it across scenes
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tourist_Ship_Movement_Ref.BeginJourney();
    }
}
