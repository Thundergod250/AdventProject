using TMPro;
using UnityEngine;
using System.Collections;

public class UI_Main_Timer : MonoBehaviour
{
    public GameObject MineTimePanel;
    [SerializeField] private TextMeshProUGUI timerText;

    private Coroutine timerRoutine;

    public void StartTimer(int value)
    {
        // Stop existing timer if running
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        timerRoutine = StartCoroutine(TimerCoroutine(value));
    }

    private IEnumerator TimerCoroutine(int duration)
    {
        MineTimePanel.SetActive(true);

        float timeRemaining = duration;

        while (timeRemaining > 0)
        {
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        timerText.text = "0";

        MineTimePanel.SetActive(false);
        timerRoutine = null;
    }
}
