using System.Threading.Tasks;// alone is not enough
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WoodChopping : MonoBehaviour
{
    public TextMeshProUGUI sequenceText;

    public GameObject buttonUI;
    public GameObject buttonPrefab;

    [SerializeField] private List<KeyCode> sequence = new List<KeyCode>();
    private List<GameObject> spawnedButtons = new List<GameObject>();
    private float inputTime = 1f;

    public async void Prompt()
    {
        await PromptRoutineAsync();
    }

    async Task PromptRoutineAsync()
    {
        GenerateSequence();

        // Show sequence for 1 second
        sequenceText.text = GetSequenceString();
        sequenceText.gameObject.SetActive(true);
        SpawnButtons();

        buttonUI.SetActive(false);

        await Task.Delay(1000); // 1 second

        // Hide sequence, show buttons
        sequenceText.gameObject.SetActive(false);
        buttonUI.SetActive(true);


        await CheckInputAsync();

        buttonUI.SetActive(false);
    }

    void GenerateSequence()
    {
        sequence.Clear();
        KeyCode[] keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

        for (int i = 0; i < 4; i++)
        {
            sequence.Add(keys[Random.Range(0, keys.Length)]);
        }
    }

    void SpawnButtons()
    {
        // Clear old buttons
        foreach (GameObject btn in spawnedButtons)
            Destroy(btn);

        spawnedButtons.Clear();

        // Spawn one button per letter
        foreach (KeyCode key in sequence)
        {
            GameObject button = Instantiate(buttonPrefab, buttonUI.transform);
            spawnedButtons.Add(button);

            TMPro.TextMeshProUGUI text =
                button.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            text.text = key.ToString();
        }
    }

    string GetSequenceString()
    {
        string result = "";
        foreach (var key in sequence)
        {
            result += key.ToString() + " ";
        }
        return result;
    }

    async Task CheckInputAsync()
    {
        float timer = inputTime;
        int index = 0;

        while (timer > 0f && index < sequence.Count)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(sequence[index]))
                {
                    index++;
                }
                else
                {
                    Debug.Log("Failed!");
                    return;
                }
            }

            timer -= Time.deltaTime;
            await Task.Yield(); // replaces yield return null
        }

        if (index == sequence.Count)
        {
            Debug.Log("Success!");
        }
        else
        {
            Debug.Log("Failed (timeout)");
        }
    }
}
