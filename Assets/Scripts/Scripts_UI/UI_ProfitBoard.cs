using TMPro;
using UnityEngine;

public class UI_ProfitBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI profitPerSecondText;
    [SerializeField] private CampFunding campFunding;

    private void Awake()
    {
        if (campFunding != null)
        {
            campFunding.EvtOnMoneyChange.AddListener(UpdateMoneyUI);
            campFunding.EvtOnRateChange.AddListener(UpdateRateUI);
        }
    }

    private void OnDestroy()
    {
        if (campFunding != null)
        {
            campFunding.EvtOnMoneyChange.RemoveListener(UpdateMoneyUI);
            campFunding.EvtOnRateChange.RemoveListener(UpdateRateUI);
        }
    }

    private void UpdateMoneyUI(int money)
    {
        if (mainText != null)
            mainText.text = $"{money}";
    }

    private void UpdateRateUI(int rate)
    {
        if (profitPerSecondText != null)
            profitPerSecondText.text = $"{rate}/sec";
    }
}