using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TouristShipMovement : MonoBehaviour
{
    [Header("Boat Start & End Positions")]
    [SerializeField] private Transform boatPointStart; // assign in Inspector
    // This is the variable target position you can set in Inspector or via script
    [SerializeField] private Vector3 targetPosition = new Vector3(-7.32000017f, -3.77999997f, 39.9000015f);

    public UnityEvent BridgeCall;

    [SerializeField] private float travelTime = 5f;    // time to move from start to target
    private bool isMoving = false;

    public void BeginJourney()
    {
        // Place ship at start point initially
        transform.position = boatPointStart.position;

        // Start coroutine for movement
        StartCoroutine(HandleShipMovement());
    }

    private IEnumerator HandleShipMovement()
    {
        isMoving = true;
        float elapsedTime = 0f;

        while (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / travelTime;

            // Smooth movement using Lerp from start to targetPosition
            transform.position = Vector3.Lerp(boatPointStart.position, targetPosition, t);

            if (t >= 1f)
            {
                isMoving = false;
            }

            yield return null;
        }

        BridgeCall.Invoke();
    }
}
