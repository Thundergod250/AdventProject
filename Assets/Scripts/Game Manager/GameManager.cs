using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TouristShipMovement TouristShipMovementRef;
    public TouristSpawner TouristSpawnerRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TouristShipMovementRef.BeginJourney();
    }
}
