using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TouristShipMovement : MonoBehaviour
{
    [Header("Boat Start & End Positions")]
    [SerializeField] private Transform boat_Point_Start; // assign in Inspector
    // This is the variable target position you can set in Inspector or via script
    [SerializeField] private Vector3 target_Position = new Vector3(-7.32000017f, -3.77999997f, 39.9000015f);

    public UnityEvent Bridge_Call;

    [SerializeField] private float travel_Time = 5f;    // time to move from start to target
    private bool is_Moving = false;

    public void BeginJourney()
    {
        // Place ship at start point initially
        transform.position = boat_Point_Start.position;

        // Start coroutine for movement
        StartCoroutine(HandleShipMovement());
    }

    private IEnumerator HandleShipMovement()
    {
        is_Moving = true;
        float elapsedTime = 0f;

        while (is_Moving)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / travel_Time;

            // Smooth movement using Lerp from start to targetPosition
            transform.position = Vector3.Lerp(boat_Point_Start.position, target_Position, t);

            if (t >= 1f)
            {
                is_Moving = false;
            }

            yield return null;
        }

        Bridge_Call.Invoke();
    }
}
