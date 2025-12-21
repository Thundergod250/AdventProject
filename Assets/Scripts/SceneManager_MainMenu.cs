using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene loading

public class SceneManager_MainMenu : MonoBehaviour
{
    [Header("Cinematic Settings")]
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private float moveSpeed = 2f;       // Target speed (units per second)
    [SerializeField] private float moveDuration = 3f;    // Seconds
    [SerializeField] private string mainGameSceneName = "MainGame"; // Scene to load

    private Coroutine cinematicRoutine;

    public void _StartCinematic()
    {
        if (cinematicRoutine != null)
        {
            StopCoroutine(cinematicRoutine);
        }
        cinematicRoutine = StartCoroutine(CinematicMove());
    }

    private IEnumerator CinematicMove()
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;

            // Quadratic acceleration curve (slow start → fast ramp)
            float currentSpeed = Mathf.Lerp(0f, moveSpeed, t * t);

            cameraObj.transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cinematicRoutine = null;

        // Load the main game scene after cinematic
        SceneManager.LoadScene(mainGameSceneName);
    }
}