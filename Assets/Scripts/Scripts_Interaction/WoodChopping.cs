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
    public GameObject WoodChoppingRoot;
    public GameObject buttonUI;
    public GameObject buttonPrefab;

    public PlayerInteraction PlayerInteractionVar;

    [SerializeField] private List<WASDKey> sequence = new List<WASDKey>();
    [SerializeField] private List<GameObject> spawnedButtons = new List<GameObject>();
    private float inputTime = 3f;


    public async void Prompt()
    {
        await PromptRoutineAsync();
    }

    async Task PromptRoutineAsync()
    {
        GenerateSequence();

        // Show sequence for 1 second
        WoodChoppingRoot.SetActive(true);
        buttonUI.SetActive(false);

        //SpawnButtons();

        // Hide sequence, show buttons
        buttonUI.SetActive(true);

        await Task.Delay(1000); // 1 second

        await CheckInputAsync();

        buttonUI.SetActive(false);
        WoodChoppingRoot.SetActive(false);
        GameManager.Instance.PlayerController.EnableMovement();
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

    async Task CheckInputAsync()
    {
        float timer = inputTime;
        int index = 0;

        while (timer > 0f && index < sequence.Count)
        {
            if (PlayerInteractionVar.IsCorrectInput(sequence[index]))
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
