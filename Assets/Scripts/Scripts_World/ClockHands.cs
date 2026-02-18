using UnityEngine;

public class ClockHands : MonoBehaviour
{
    [SerializeField] private GameObject shortHand; // Hour hand
    [SerializeField] private GameObject longHand;  // Minute hand
    [SerializeField] private float timeMultiplier = 1f; // Speed control

    private float minutes; // Current minutes
    private float hours;   // Current hours

    private void Update()
    {
        // Advance time
        minutes += Time.deltaTime * timeMultiplier;
        
        // When minutes reach 60, reset and increment hours
        if (minutes >= 60f)
        {
            minutes -= 60f;
            hours += 1f;
        }

        // Keep hours within 12-hour cycle
        if (hours >= 12f)
            hours -= 12f;

        // Rotate hands
        RotateHands();
    }

    private void RotateHands()
    {
        // Minute hand: 360° per 60 minutes → 6° per minute
        float minuteRotation = minutes * 6f;

        // Hour hand: 360° per 12 hours → 30° per hour
        // Also moves gradually as minutes pass (0.5° per minute)
        float hourRotation = (hours * 30f) + (minutes * 0.5f);

        if (longHand != null)
            longHand.transform.localRotation = Quaternion.Euler(0f, -minuteRotation, 0f);

        if (shortHand != null)
            shortHand.transform.localRotation = Quaternion.Euler(0f, -hourRotation, 0f);
    }
}
