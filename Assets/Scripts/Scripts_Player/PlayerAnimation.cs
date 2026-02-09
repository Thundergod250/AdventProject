using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private Transform model; // child model to animate
    [SerializeField] private float bounceHeight = 0.25f;
    [SerializeField] private float bounceSpeed = 6f;

    private Vector3 initialLocalPos;
    private float bounceTimer;
    private bool isMoving;

    private void Awake()
    {
        if (model == null)
            model = transform; // fallback if not assigned

        initialLocalPos = model.localPosition;
    }

    private void Update()
    {
        if (isMoving)
        {
            bounceTimer += Time.deltaTime * bounceSpeed;

            // 👇 Use Mathf.Abs so the bounce is always upward
            float offset = Mathf.Abs(Mathf.Sin(bounceTimer)) * bounceHeight;

            model.localPosition = initialLocalPos + Vector3.up * offset;
        }
        else
        {
            // Reset smoothly when idle
            model.localPosition = Vector3.Lerp(
                model.localPosition,
                initialLocalPos,
                Time.deltaTime * bounceSpeed
            );
            bounceTimer = 0f;
        }
    }

    // Called by PlayerMovement
    public void SetIsMoving(bool value) => isMoving = value;
}