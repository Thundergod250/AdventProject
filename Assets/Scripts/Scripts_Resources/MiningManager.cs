using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MiningManager : MonoBehaviour
{
    [Header("Prefabs & References")]
    [SerializeField] private List<GameObject> rockPrefabs; // assign multiple rock prefabs in Inspector
    [SerializeField] private GameObject blockingWall;
    [SerializeField] private UI_Main_Timer ui_Main_TimerObject;
    [SerializeField] private UI_Mining ui_MiningObject;
    public GameObject Player; // assign Player in Inspector
    public Interactable Interactable;

    [Header("Spawn Settings")]
    [SerializeField] private Vector2 planeSize = new Vector2(10f, 10f);
    [SerializeField] private int rockCount = 10;
    [SerializeField] private float groundY = 0f;

    [Header("Enemy Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs; // assign multiple enemy prefabs in Inspector
    [SerializeField] private Transform[] enemySpawnPoints;

    private int currentEnemyIndex = 0; // tracks which enemy prefab to use

    [Header("Game Settings")]
    [SerializeField] private float miningDuration = 10f;

    [Header("Upgrade Settings")]
    [SerializeField] private int currentPrice;

    // Separate upgrade prices
    [SerializeField] private int mineDurationUpgradePrice = 1;
    [SerializeField] private int destroyablesUpgradePrice;
    [SerializeField] private int upgradePriceIncrease = 10;
    private int upgradeCallCount = 0; // tracks how many times UpgradeMineQuality was called

    public int MineLevel { get; private set; } = 1;

    private bool isMiningActive = false;
    private Coroutine miningRoutine;

    private List<GameObject> spawnedRocks = new List<GameObject>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private int currentRockIndex = 0; // tracks which rock prefab to use

    private void Awake()
    {
        //upgradePrice = currentPrice;
        upgradeCallCount = 1;
        destroyablesUpgradePrice = GetUpgradeIncrement(upgradeCallCount);
        UpdateUITexts();
    }

    public void SetUI()
    {
        ui_MiningObject.ToggleMiningUI();
    }

    public void StartMining()
    {
        if (isMiningActive) return;
        miningRoutine = StartCoroutine(MiningSession());
    }
    private GameObject SpawnPrefab(GameObject prefab, Vector3 position, Quaternion rotation, List<GameObject> list)
    {
        GameObject obj = Instantiate(prefab, position, rotation);
        list.Add(obj);
        return obj;
    }

    private IEnumerator MiningSession()
    {
        isMiningActive = true;

        ui_Main_TimerObject.StartTimer((int)miningDuration);

        if (blockingWall != null)
            blockingWall.SetActive(true);

        Interactable.IsInteractable = false;

        // Spawn rocks
        for (int i = 0; i < rockCount; i++)
        {
            Vector3 spawnPos = GetRandomGroundPosition();
            if (rockPrefabs.Count > 0 && currentRockIndex < rockPrefabs.Count)
            {
                SpawnPrefab(rockPrefabs[currentRockIndex], spawnPos, Quaternion.identity, spawnedRocks);
            }
        }

        // Spawn enemies
        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            if (enemyPrefabs.Count > 0 && currentEnemyIndex < enemyPrefabs.Count && spawnPoint != null)
            {
                SpawnPrefab(enemyPrefabs[currentEnemyIndex], spawnPoint.position, spawnPoint.rotation, spawnedEnemies);
            }
        }

        yield return new WaitForSeconds(miningDuration);

        Interactable.IsInteractable = true;

        Cleanup();

        isMiningActive = false;
        miningRoutine = null;
    }

    public void StopMining()
    {
        if (miningRoutine != null)
        {
            StopCoroutine(miningRoutine);
            miningRoutine = null;
        }

        Cleanup();
        isMiningActive = false;
    }

    private void Cleanup()
    {
        DestroyAll(spawnedRocks);
        DestroyAll(spawnedEnemies);

        if (blockingWall != null)
            blockingWall.SetActive(false);

        ui_Main_TimerObject?.StopTimer();
    }

    public void UpgradeMineDuration()
    {
        if (GameManager.Instance.GoldManager.HasEnoughGold(mineDurationUpgradePrice))
        {
            GameManager.Instance.GoldManager.SpendGold(mineDurationUpgradePrice);

            miningDuration += 1f;
            mineDurationUpgradePrice += 1;

            ui_MiningObject.UpdateTimerLevel(miningDuration);

            // Update UI price
            ui_MiningObject.UpdateMineDurationPrice(mineDurationUpgradePrice);
        }
        else
        {
            StartCoroutine(ClearNotEnoughMineDurationText());
        }
    }

    public void UpgradeMineQuality()
    {
        if (GameManager.Instance.GoldManager.HasEnoughGold(destroyablesUpgradePrice))
        {
            GameManager.Instance.GoldManager.SpendGold(destroyablesUpgradePrice);

            UpgradeRocks();
            UpgradeEnemies();

            // Increment call count
            upgradeCallCount++;

            // Get increment from helper function
            int increment = GetUpgradeIncrement(upgradeCallCount);

            destroyablesUpgradePrice = increment;
            Debug.LogWarning($"destroyablesUpgradePrice: {destroyablesUpgradePrice}");

            // Update UI price
            ui_MiningObject.UpdateMineQualityPrice(destroyablesUpgradePrice);

            // Optional: trigger boss summon when increment hits 50
            if (destroyablesUpgradePrice == 50)
            {
                Debug.Log("Boss summoned!");
                // Add your boss summon logic here
            }
        }
        else
        {
            StartCoroutine(ClearNotEnoughMineQualityText());
        }
    }

    /// Returns the upgrade increment based on how many times the upgrade has been called.
    /// Progression: 10 → 20 → 40 → 50 (max).
    private int GetUpgradeIncrement(int callCount)
    {
        switch (callCount)
        {
            case 1: return 10;
            case 2: return 20;
            case 3: return 40;
            default: return 50; // cap at 50
        }
    }

    private void UpgradeRocks()
    {
        if (currentRockIndex < rockPrefabs.Count - 1)
        {
            currentRockIndex++;
            MineLevel++;

            ui_MiningObject.UpdateRockLevel(currentRockIndex);
        } 
        else
            Debug.LogWarning("Max Level Rocks Reached");
    }

    private void UpgradeEnemies()
    {
        if (currentEnemyIndex < enemyPrefabs.Count - 1)
        {
            currentEnemyIndex++;

            ui_MiningObject.UpdateEnemyLevel(currentEnemyIndex);
        }
        else
            Debug.LogWarning("Max Level Enemies Reached");
    }

    private void DestroyAll(List<GameObject> list)
    {
        foreach (var obj in list)
        {
            if (obj != null) Destroy(obj);
        }
        list.Clear();
    }

    private void UpdateUITexts()
    {
        if (ui_MiningObject != null)
        {
            // Update prices
            ui_MiningObject.UpdateMineDurationPrice(mineDurationUpgradePrice);
            ui_MiningObject.UpdateMineQualityPrice(GetUpgradeIncrement(upgradeCallCount));

            // Update levels
            ui_MiningObject.UpdateRockLevel(currentRockIndex);
            ui_MiningObject.UpdateEnemyLevel(currentEnemyIndex);

            //Update Time
            ui_MiningObject.UpdateTimerLevel(miningDuration);
        }
    }

    private IEnumerator ClearNotEnoughMineDurationText()
    {
        if (ui_MiningObject.MineDurationButton == null || ui_MiningObject.UiMineDurationLabelText == null) yield break;

        // Disable button and show "Not enough money"
        ui_MiningObject.MineDurationButton.interactable = false;
        TextMeshProUGUI buttonText = ui_MiningObject.MineDurationButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null) buttonText.text = "Not enough money";

        yield return new WaitForSeconds(1f);

        // Re-enable button and restore label
        ui_MiningObject.MineDurationButton.interactable = true;
        if (buttonText != null) buttonText.text = $"Upgrade Mine Duration";
    }

    private IEnumerator ClearNotEnoughMineQualityText()
    {
        if (ui_MiningObject.MineQualityButton == null || ui_MiningObject.UiMineQualityLabelText == null || ui_MiningObject.UiMineQualityPriceText == null) yield break;

        // Disable button and show "Not enough money"
        ui_MiningObject.MineQualityButton.interactable = false;
        TextMeshProUGUI buttonText = ui_MiningObject.MineQualityButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null) buttonText.text = "Not enough money";

        yield return new WaitForSeconds(1f);

        // Re-enable button and restore label
        ui_MiningObject.MineQualityButton.interactable = true;
        if (buttonText != null) buttonText.text = $"Upgrade Mine Quality";
    }

    private Vector3 GetRandomGroundPosition()
    {
        float x = Random.Range(-planeSize.x * 0.5f, planeSize.x * 0.5f);
        float z = Random.Range(-planeSize.y * 0.5f, planeSize.y * 0.5f);

        return transform.position + new Vector3(x, groundY, z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = transform.position + new Vector3(0f, groundY, 0f);
        Vector3 size = new Vector3(planeSize.x, 0.1f, planeSize.y);
        Gizmos.DrawWireCube(center, size);
    }
}
