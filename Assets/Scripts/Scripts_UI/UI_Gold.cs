using System;
using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    private TextMeshProUGUI goldText;

    private void Start()
    {
        goldText = GetComponent<TextMeshProUGUI>();
        if (goldText == null) 
            Debug.LogError($"UI_Gold on {gameObject.name} requires a TextMeshProUGUI component.");
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null. UI_Gold cannot subscribe to gold updates.");
            return;
        }
        
        if (GameManager.Instance.GoldManager == null)
        {
            Debug.LogError("GoldManager is missing in GameManager. UI_Gold cannot subscribe to gold updates.");
            return;
        }
        
        if (GameManager.Instance.GoldManager.EvtOnGoldChanged == null)
        {
            Debug.LogError("EvtOnGoldChanged UnityEvent is not initialized in GoldManager.");
            return;
        }
        
        GameManager.Instance.GoldManager.EvtOnGoldChanged.AddListener(UpdateGoldText);
        
        UpdateGoldText(GameManager.Instance.GoldManager.PlayerGold);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.GoldManager != null) 
            GameManager.Instance.GoldManager.EvtOnGoldChanged.RemoveListener(UpdateGoldText);
    }

    private void UpdateGoldText(int value)
    {
        if (goldText != null)
            goldText.text = value.ToString();
    }
}