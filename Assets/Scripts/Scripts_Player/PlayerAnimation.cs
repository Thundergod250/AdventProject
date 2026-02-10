using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private Transform model; 
    [SerializeField] private float bounceHeight = 0.25f;
    [SerializeField] private float bounceSpeed = 6f;

    [Header("Slam Settings")]
    [SerializeField] private float slamLiftHeight = 1.5f;
    [SerializeField] private float slamHoldTime = 0.1f; // very short pause
    [SerializeField] private float liftDuration = 0.2f; // quick rise
    [SerializeField] private float slamDuration = 0.1f; // explosive slam
    public ParticleSystem slamVFX;

    private Vector3 initialLocalPos;
    private float bounceTimer;
    private bool isMoving;
    private bool isSlamming;

    private void Awake()
    {
        if (model == null)
            model = transform;

        initialLocalPos = model.localPosition;
    }

    private void Update()
    {
        if (isSlamming) return; // freeze bounce during slam

        if (isMoving)
        {
            bounceTimer += Time.deltaTime * bounceSpeed;
            float offset = Mathf.Abs(Mathf.Sin(bounceTimer)) * bounceHeight;
            model.localPosition = initialLocalPos + Vector3.up * offset;
        }
        else
        {
            model.localPosition = Vector3.Lerp(model.localPosition, initialLocalPos, Time.deltaTime * bounceSpeed);
            bounceTimer = 0f;
        }
    }

    public void SetIsMoving(bool value) => isMoving = value;

    // Slam coroutine
    public IEnumerator PlaySlam()
    {
        isSlamming = true;
        isMoving = false;

        Vector3 liftedPos = initialLocalPos + Vector3.up * slamLiftHeight;

        // Lift up quickly
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / liftDuration;
            model.localPosition = Vector3.Lerp(initialLocalPos, liftedPos, t);
            yield return null;
        }

        // Hold briefly
        yield return new WaitForSeconds(slamHoldTime);

        // Slam down explosively
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / slamDuration;
            // SmoothStep exaggerates acceleration
            model.localPosition = Vector3.Lerp(liftedPos, initialLocalPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        // Play VFX
        if (slamVFX != null)
            slamVFX.Play();

        // Optional squash/stretch
        StartCoroutine(SquashStretch());

        isSlamming = false;
    }

    private IEnumerator SquashStretch()
    {
        Vector3 normalScale = model.localScale;
        Vector3 squashedScale = new Vector3(normalScale.x * 1.2f, normalScale.y * 0.8f, normalScale.z * 1.2f);

        model.localScale = squashedScale;
        yield return new WaitForSeconds(0.1f);
        model.localScale = normalScale;
    }
}
