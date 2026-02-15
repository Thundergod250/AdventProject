using UnityEngine;

public class Boss_SapGuardian : MonoBehaviour
{
    [Header("Appearance Reference")]
    [SerializeField] private Transform appearance; // mesh only, not colliders

    [Header("Bobbing Settings")]
    [SerializeField] private float bobHeight = 0.5f;   // how high it goes up
    [SerializeField] private float bobSpeed = 2f;      // how fast it bobs

    [Header("Target Reference")]
    [SerializeField] private Transform player;         // assign player transform

    private Vector3 basePosition;

    private void Start()
    {
        if (appearance == null)
        {
            Debug.LogError("Boss appearance not assigned!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player reference not assigned!");
            return;
        }

        // Save starting position (ground level)
        basePosition = appearance.localPosition;
    }

    private void Update()
    {
        HandleTargetingBobbing();
        FacePlayer();
    }

    private void HandleTargetingBobbing()
    {
        // Simple sine wave bobbing
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        appearance.localPosition = basePosition + Vector3.up * offset;
    }

    private void FacePlayer()
    {
        // Rotate boss to face player horizontally
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // keep rotation flat on ground plane

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 5f // rotation speed
            );
        }
    }
}