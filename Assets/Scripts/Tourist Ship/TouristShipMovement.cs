using UnityEngine;

public class TouristShipMovement : MonoBehaviour
{
    [SerializeField] private Transform boatPointStart; // assign in Inspector
    [SerializeField] private Transform boatPointEnd;   // assign in Inspector
    [SerializeField] private float travelTime = 5f;    // time to move from start to end

    private float elapsedTime = 0f;
    private bool isMoving = false;

    private void Start()
    {
        // Place ship at start point initially
        transform.position = boatPointStart.position;

        BeginJourney();
    }

    private void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / travelTime;

            // Smooth movement using Lerp
            transform.position = Vector3.Lerp(boatPointStart.position, boatPointEnd.position, t);

            // Stop once we reach the end
            if (t >= 1f)
            {
                isMoving = false;
            }
        }
    }

    // Call this method to start the ship’s journey
    public void BeginJourney()
    {
        elapsedTime = 0f;
        isMoving = true;
    }
}
