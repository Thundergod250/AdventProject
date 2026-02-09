using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private Transform model; 
    [SerializeField] private float bounceHeight = 0.25f;
    [SerializeField] private float bounceSpeed = 6f;

    [Header("Slam Settings")]
    [SerializeField] private float slamLiftHeight = 1f;
    [SerializeField] private float slamHoldTime = 1f;
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

        // Lift up
        Vector3 liftedPos = initialLocalPos + Vector3.up * slamLiftHeight;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            model.localPosition = Vector3.Lerp(initialLocalPos, liftedPos, t);
            yield return null;
        }

        // Hold
        yield return new WaitForSeconds(slamHoldTime);

        // Slam down
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            model.localPosition = Vector3.Lerp(liftedPos, initialLocalPos, t);
            yield return null;
        }

        // Play VFX
        if (slamVFX != null)
            slamVFX.Play();

        isSlamming = false;
    }
}
