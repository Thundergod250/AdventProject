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

    [Header("Upgrade UI")]
    [SerializeField] private TextMeshProUGUI uiUpgradePriceText;
    [SerializeField] private GameObject uiUpgradeLabelText;

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
    [SerializeField] private int upgradePriceIncrease = 30;
    public int MineLevel = 1; // tracks current mining level

    private bool isMiningActive = false;
    private Coroutine miningRoutine;

    private List<GameObject> spawnedRocks = new List<GameObject>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int upgradePrice;

    private int currentRockIndex = 0; // tracks which rock prefab to use

    private void Awake()
    {
        upgradePrice = currentPrice;
        UpdateUpgradePriceText();
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

    private IEnumerator MiningSession()
    {
        isMiningActive = true;

        ui_Main_TimerObject.StartTimer((int)miningDuration);

        if (blockingWall != null)
            blockingWall.SetActive(true);

        // Spawn rocks using current prefab
        for (int i = 0; i < rockCount; i++)
        {
            Vector3 spawnPos = GetRandomGroundPosition();
            if (rockPrefabs.Count > 0 && currentRockIndex < rockPrefabs.Count)
            {
                GameObject rock = Instantiate(rockPrefabs[currentRockIndex], spawnPos, Quaternion.identity);
                spawnedRocks.Add(rock);
            }
        }

        // Spawn enemies
        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            if (enemyPrefabs.Count > 0 && currentEnemyIndex < enemyPrefabs.Count && spawnPoint != null)
            {
                GameObject enemy = Instantiate(enemyPrefabs[currentEnemyIndex], spawnPoint.position, spawnPoint.rotation);
                spawnedEnemies.Add(enemy);
            }
        }

        yield return new WaitForSeconds(miningDuration);

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
        foreach (var rock in spawnedRocks)
        {
            if (rock != null) Destroy(rock);
        }
        spawnedRocks.Clear();

        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        spawnedEnemies.Clear();

        if (blockingWall != null)
            blockingWall.SetActive(false);

        if (ui_Main_TimerObject != null)
            ui_Main_TimerObject.StopTimer();
    }

    public void Upgrade()
    {
        if (GameManager.Instance.GoldManager.HasEnoughGold(upgradePrice))
        {
            GameManager.Instance.GoldManager.SpendGold(upgradePrice);

            miningDuration += 10f;
            upgradePrice += upgradePriceIncrease;

            // Move to next rock prefab if available
            if (currentRockIndex < rockPrefabs.Count - 1)
            {
                currentRockIndex++;
                MineLevel++;
                Debug.Log($"Rock type upgraded to index {currentRockIndex}, level {MineLevel}");
            }

            // Move to next enemy prefab if available
            if (currentEnemyIndex < enemyPrefabs.Count - 1)
            {
                currentEnemyIndex++;
                Debug.Log($"Enemy type upgraded to index {currentEnemyIndex}");
            }

            UpdateUpgradePriceText();
        }
        else
        {
            StartCoroutine(ClearNotEnoughText());
        }
    }



    private void UpdateUpgradePriceText()
    {
        if (uiUpgradePriceText != null)
            uiUpgradePriceText.text = upgradePrice.ToString();
    }

    private IEnumerator ClearNotEnoughText()
    {
        if (uiUpgradeLabelText == null) yield break;

        uiUpgradeLabelText.SetActive(true);
        yield return new WaitForSeconds(1f);
        uiUpgradeLabelText.SetActive(false);
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
