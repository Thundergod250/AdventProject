using System.Collections.Generic;
using System.Threading.Tasks;// alone is not enough
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WASDKey
{
    W,
    A,
    S,
    D
}

public class WoodChopping : MonoBehaviour
{
    public GameObject buttonUI;
    public GameObject buttonPrefab;

    public InputActionReference inputW;
    public InputActionReference inputA;
    public InputActionReference inputS;
    public InputActionReference inputD;

    [SerializeField] private List<WASDKey> sequence = new List<WASDKey>();
    [SerializeField] private List<GameObject> spawnedButtons = new List<GameObject>();
    private float inputTime = 3f;

    void OnEnable()
    {
        inputW.action.Enable();
        inputA.action.Enable();
        inputS.action.Enable();
        inputD.action.Enable();
    }

    public async void Prompt()
    {
        await PromptRoutineAsync();
    }

    async Task PromptRoutineAsync()
    {
        GenerateSequence();

        // Show sequence for 1 second

        buttonUI.SetActive(false);

        //SpawnButtons();

        await Task.Delay(1000); // 1 second

        // Hide sequence, show buttons
        buttonUI.SetActive(true);

        await CheckInputAsync();

        buttonUI.SetActive(false);
    }

    void GenerateSequence()
    {
        sequence.Clear();

        // Clear old buttons
        foreach (GameObject btn in spawnedButtons)
            Destroy(btn);

        spawnedButtons.Clear();

        WASDKey[] keys = { WASDKey.W, WASDKey.A, WASDKey.S, WASDKey.D };

        for (int i = 0; i < 4; i++)
        {
            // Generate key
            WASDKey key = keys[Random.Range(0, keys.Length)];
            sequence.Add(key);

            // Spawn UI
            GameObject ui = Instantiate(buttonPrefab, buttonUI.transform);
            spawnedButtons.Add(ui);

            // Set text (Image → Text)
            TextMeshProUGUI tmp = ui.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = key.ToString();
            }
            else
            {
                Debug.LogError("Button prefab missing TextMeshProUGUI child!");
            }
        }
    }

    bool IsCorrectInput(WASDKey key)
    {
        return key switch
        {
            WASDKey.W => inputW.action.WasPressedThisFrame(),
            WASDKey.A => inputA.action.WasPressedThisFrame(),
            WASDKey.S => inputS.action.WasPressedThisFrame(),
            WASDKey.D => inputD.action.WasPressedThisFrame(),
            _ => false
        };
    }

    async Task CheckInputAsync()
    {
        float timer = inputTime;
        int index = 0;

        while (timer > 0f && index < sequence.Count)
        {
            if (IsCorrectInput(sequence[index]))
            {
                Debug.LogWarning("Succeeded");
                index++; // move to next letter
            }
            else 
            {
                Debug.Log("Failed!");
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
