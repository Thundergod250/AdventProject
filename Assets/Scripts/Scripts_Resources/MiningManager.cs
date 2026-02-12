using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MiningManager : MonoBehaviour
{
    [Header("Prefabs & References")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject blockingWall;
    [SerializeField] private TextMeshProUGUI uiMiningCountdown;

    [Header("Upgrade UI")]
    [SerializeField] private TextMeshProUGUI uiUpgradePriceText;
    [SerializeField] private GameObject uiUpgradeLabelText; // optional, if you want to reference the "Upgrade" label
    [SerializeField] private UI_Main_Timer ui_Main_TimerObject;

    [Header("Spawn Settings")]
    [SerializeField] private Vector2 planeSize = new Vector2(10f, 10f); // X/Z area
    [SerializeField] private int rockCount = 10; // how many rocks to spawn
    [SerializeField] private float groundY = 0f; // fixed ground height

    [Header("Game Settings")]
    [SerializeField] private float miningDuration = 10f; // seconds
    private bool isMiningActive = false;

    private List<GameObject> spawnedRocks = new List<GameObject>();
    private Coroutine miningRoutine;

    [SerializeField] private int currentPrice;
    [SerializeField] private int upgradePrice;

    private void Awake()
    {
        upgradePrice = currentPrice;
    }

    // === Public Entry Point ===
    public void StartMining()
    {
        if (isMiningActive) return; // prevent re-entry while active
        miningRoutine = StartCoroutine(MiningSession());
    }

    // === Upgrade Button Entry Point ===
    public void Upgrade() 
    {
        if (GameManager.Instance.GoldManager.playerGold >= upgradePrice)
        { 
            GameManager.Instance.GoldManager.playerGold -= upgradePrice;
            upgradePrice += 30; // increment price
            miningDuration += 10f;
            UpdateUpgradePriceText();
        }
        else
        { 
            StartCoroutine(ClearNotEnoughText());      
            return;
        }
    }

    public void UpdateUpgradePriceText() 
    { 
        if (uiUpgradePriceText != null) 
            uiUpgradePriceText.text = upgradePrice.ToString(); 
    }

    // === Core Mining Flow ===
    private IEnumerator MiningSession()
    {
        isMiningActive = true;

        ui_Main_TimerObject.MineTimePanel.SetActive(true);
        // Enable wall
        if (blockingWall != null)
            blockingWall.SetActive(true);

        // Spawn rocks
        for (int i = 0; i < rockCount; i++)
        {
            Vector3 spawnPos = GetRandomGroundPosition();
            GameObject rock = Instantiate(rockPrefab, spawnPos, Quaternion.identity);
            spawnedRocks.Add(rock);
        }

        //Countdown loop 
         float remainingTime = miningDuration; 
        
        while (remainingTime > 0f) 
        { 
            if (uiMiningCountdown != null) uiMiningCountdown.text = Mathf.CeilToInt(remainingTime).ToString(); 
            yield return null; 
            
            // wait one frame
            remainingTime -= Time.deltaTime; 
        } 
        
        // Clear UI text when finished
        if (uiMiningCountdown != null) 
            uiMiningCountdown.text = string.Empty;

        // Cleanup
        foreach (var rock in spawnedRocks)
        {
            if (rock != null)
                Destroy(rock);
        }
        spawnedRocks.Clear();

        if (blockingWall != null)
            blockingWall.SetActive(false);

        ui_Main_TimerObject.MineTimePanel.SetActive(false);
        isMiningActive = false;
        miningRoutine = null;
    }

    private IEnumerator ClearNotEnoughText()
    {
        uiUpgradeLabelText.SetActive(!uiUpgradeLabelText.activeSelf);
        yield return new WaitForSeconds(1);
        uiUpgradeLabelText.SetActive(!uiUpgradeLabelText.activeSelf);
    }

    // === Helpers ===
    private Vector3 GetRandomGroundPosition()
    {
        float x = Random.Range(-planeSize.x * 0.5f, planeSize.x * 0.5f);
        float z = Random.Range(-planeSize.y * 0.5f, planeSize.y * 0.5f);

        // Offset by MiningManager's transform position
        return transform.position + new Vector3(x, groundY, z);
    }


    // === Gizmos ===
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = transform.position + new Vector3(0f, groundY, 0f);
        Vector3 size = new Vector3(planeSize.x, 0.1f, planeSize.y);
        Gizmos.DrawWireCube(center, size);
    }
}
