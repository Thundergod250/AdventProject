using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UI_FadeGameOverScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image gameOverImage;       // assign your black Image
    [SerializeField] private TMP_Text gameOverText;     // assign your child Text (TMP_Text)
    [SerializeField] private float fadeDuration = 1f;   // how long the fade takes

    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (gameOverImage == null)
            gameOverImage = GetComponent<Image>();

        // Ensure invisible at start
        SetAlpha(0f);
        gameOverImage.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverImage.gameObject.SetActive(true);
        StartFade(1f); // fade to fully visible
    }

    public void HideGameOver()
    {
        StartFade(0f, disableOnComplete: true); // fade to invisible and disable
    }

    private void StartFade(float targetAlpha, bool disableOnComplete = false)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha, disableOnComplete));
    }

    private IEnumerator FadeRoutine(float targetAlpha, bool disableOnComplete)
    {
        float startAlpha = gameOverImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha);

        if (disableOnComplete && targetAlpha == 0f)
            gameOverImage.gameObject.SetActive(false);

        fadeRoutine = null;
    }

    private void SetAlpha(float alpha)
    {
        // Fade Image
        if (gameOverImage != null)
        {
            Color c = gameOverImage.color;
            c.a = alpha;
            gameOverImage.color = c;
        }

        // Fade Text
        if (gameOverText != null)
        {
            Color t = gameOverText.color;
            t.a = alpha;
            gameOverText.color = t;
        }
    }
}
