using UnityEngine;
using System.Collections;

public class HittingBoulder : MonoBehaviour
{
    [SerializeField] private float dropAmount = 2.5f; // configurable in Inspector
    [SerializeField] private float resetDelay = 3f;   // time before resetting

    private Vector3 startPosition; // store starting position
    private Coroutine resetRoutine;

    private void Start()
    {
        // Save the initial position when the game starts
        startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has a Boss_SapGuardian component
        Boss_SapGuardian boss = other.GetComponent<Boss_SapGuardian>();
        if (boss != null)
        {
            // Lower THIS boulder's Y position by dropAmount
            Vector3 pos = transform.position;
            pos.y -= dropAmount;
            transform.position = pos;

            // If a reset coroutine is already running, stop it
            if (resetRoutine != null)
            {
                StopCoroutine(resetRoutine);
            }

            // Start a new reset coroutine
            resetRoutine = StartCoroutine(ResetPositionAfterDelay());
        }
    }

    private IEnumerator ResetPositionAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(resetDelay);

        // Reset back to the original starting position
        transform.position = startPosition;

        resetRoutine = null; // clear reference
    }
}
