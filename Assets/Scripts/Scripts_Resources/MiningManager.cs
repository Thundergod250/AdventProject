using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MiningManager : MonoBehaviour
{
    [Header("Prefabs & References")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject enemyPrefab;          // enemy prefab
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
    [SerializeField] private Transform[] enemySpawnPoints;    // assign spawn points in Inspector

    [Header("Game Settings")]
    [SerializeField] private float miningDuration = 10f;

    [Header("Upgrade Settings")]
    [SerializeField] private int currentPrice;
    [SerializeField] private int upgradePriceIncrease = 30;

    private bool isMiningActive = false;
    private Coroutine miningRoutine;

    private List<GameObject> spawnedRocks = new List<GameObject>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int upgradePrice;

    private void Awake()
    {
        upgradePrice = currentPrice;
        UpdateUpgradePriceText();
    }

    public void SetUI()
    {
        ui_MiningObject.ToggleMiningUI();
    }

    // =====================================================
    // PUBLIC ENTRY POINT
    // =====================================================
    public void StartMining()
    {
        if (isMiningActive) return;

        miningRoutine = StartCoroutine(MiningSession());
    }

    // =====================================================
    // CORE MINING FLOW
    // =====================================================
    private IEnumerator MiningSession()
    {
        isMiningActive = true;

        // Start UI timer
        ui_Main_TimerObject.StartTimer((int)miningDuration);

        // Enable blocking wall
        if (blockingWall != null)
            blockingWall.SetActive(true);

        // Spawn rocks
        for (int i = 0; i < rockCount; i++)
        {
            Vector3 spawnPos = GetRandomGroundPosition();
            GameObject rock = Instantiate(rockPrefab, spawnPos, Quaternion.identity);
            spawnedRocks.Add(rock);
        }

        // Spawn enemies at each spawn point
        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            if (enemyPrefab != null && spawnPoint != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedEnemies.Add(enemy);
            }
        }

        // Wait for mining duration
        yield return new WaitForSeconds(miningDuration);

        Cleanup();

        isMiningActive = false;
        miningRoutine = null;
    }

    // =====================================================
    // STOP MINING FUNCTION
    // =====================================================
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

    // =====================================================
    // CLEANUP FUNCTION
    // =====================================================
    private void Cleanup()
    {
        // Cleanup rocks
        foreach (var rock in spawnedRocks)
        {
            if (rock != null) Destroy(rock);
        }
        spawnedRocks.Clear();

        // Cleanup enemies
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        spawnedEnemies.Clear();

        // Disable blocking wall
        if (blockingWall != null)
            blockingWall.SetActive(false);

        // Reset UI timer
        if (ui_Main_TimerObject != null)
            ui_Main_TimerObject.StopTimer();
    }

    // =====================================================
    // UPGRADE LOGIC
    // =====================================================
    public void Upgrade()
    {
        if (GameManager.Instance.GoldManager.playerGold >= upgradePrice)
        {
            GameManager.Instance.GoldManager.playerGold -= upgradePrice;

            miningDuration += 10f;
            upgradePrice += upgradePriceIncrease;

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

    // =====================================================
    // HELPER METHODS
    // =====================================================
    private Vector3 GetRandomGroundPosition()
    {
        float x = Random.Range(-planeSize.x * 0.5f, planeSize.x * 0.5f);
        float z = Random.Range(-planeSize.y * 0.5f, planeSize.y * 0.5f);

        return transform.position + new Vector3(x, groundY, z);
    }

    // =====================================================
    // GIZMOS
    // =====================================================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = transform.position + new Vector3(0f, groundY, 0f);
        Vector3 size = new Vector3(planeSize.x, 0.1f, planeSize.y);
        Gizmos.DrawWireCube(center, size);
    }
}
